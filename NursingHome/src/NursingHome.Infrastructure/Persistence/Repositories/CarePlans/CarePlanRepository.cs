using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Abstractions.CarePlans;
using NursingHome.Application.Features.CarePlans.DTOs;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Infrastructure.Persistence.Generated;

namespace NursingHome.Infrastructure.Persistence.Repositories.CarePlans;

public class CarePlanRepository(NursingHomeDbContext context) : ICarePlanRepository
{
    private const string DefaultTaskType = "Care task";

    private readonly NursingHomeDbContext _context = context;

    public Task<bool> ResidentExistsAsync(long residentId, CancellationToken cancellationToken = default)
        => _context.Set<Resident>().AnyAsync(r => r.Id == residentId && !r.IsDeleted, cancellationToken);

    public async Task<bool> IsResidentChartLockedAsync(long residentId, CancellationToken cancellationToken = default)
        => await _context.Set<Resident>()
            .Where(r => r.Id == residentId)
            .Select(r => r.IsChartLocked)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<CarePlanDetailDto> CreateDraftAsync(
        long residentId, bool significantChangeFlag, IReadOnlyList<CareAreaInput> areas, CancellationToken cancellationToken = default)
    {
        var plan = CarePlan.CreateDraft(residentId, significantChangeFlag);
        var built = AttachCareAreas(plan, areas);

        _context.Set<CarePlan>().Add(plan);
        await _context.SaveChangesAsync(cancellationToken);

        return MapToDetail(plan, built);
    }

    public async Task<CarePlanSnapshot?> GetSnapshotAsync(long planId, CancellationToken cancellationToken = default)
        => await _context.Set<CarePlan>()
            .Where(p => p.Id == planId)
            .Select(p => new CarePlanSnapshot(
                p.Id,
                p.Status,
                p.IsDeleted,
                p.CareGoals.Count,
                p.CareInterventions.Count))
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<CarePlanDetailDto> ReplaceCareAreasAsync(
        long planId, IReadOnlyList<CareAreaInput> areas, CancellationToken cancellationToken = default)
    {
        var plan = await _context.Set<CarePlan>()
            .Include(p => p.CareGoals)
            .Include(p => p.CareInterventions)
            .FirstOrDefaultAsync(p => p.Id == planId && !p.IsDeleted, cancellationToken)
            ?? throw new KeyNotFoundException($"Care plan {planId} not found.");

        // DRAFT chưa sinh task nên xóa goals/interventions cũ an toàn.
        _context.Set<CareGoal>().RemoveRange(plan.CareGoals.ToList());
        _context.Set<CareIntervention>().RemoveRange(plan.CareInterventions.ToList());

        var built = AttachCareAreas(plan, areas);
        plan.Touch();

        await _context.SaveChangesAsync(cancellationToken);

        return MapToDetail(plan, built);
    }

    public async Task<int> ActivateAsync(long planId, CancellationToken cancellationToken = default)
    {
        var plan = await _context.Set<CarePlan>()
            .Include(p => p.CareInterventions)
            .FirstOrDefaultAsync(p => p.Id == planId && !p.IsDeleted, cancellationToken)
            ?? throw new KeyNotFoundException($"Care plan {planId} not found.");

        var now = DateTimeOffset.UtcNow;
        var generated = 0;

        foreach (var intervention in plan.CareInterventions)
        {
            intervention.CareTasks.Add(CareTask.CreatePending(DefaultTaskType, now));
            generated++;
        }

        plan.Activate();
        await _context.SaveChangesAsync(cancellationToken);

        return generated;
    }

    // Gắn goals + interventions vào plan (chưa save) và trả cấu trúc để echo lại text đầu vào.
    private static List<BuiltArea> AttachCareAreas(CarePlan plan, IReadOnlyList<CareAreaInput> areas)
    {
        var built = new List<BuiltArea>();

        foreach (var area in areas)
        {
            var goal = CareGoal.Create();
            plan.CareGoals.Add(goal);

            var interventions = new List<(CareIntervention Entity, CarePlanInterventionInput Input)>();
            foreach (var ivInput in area.Interventions ?? new List<CarePlanInterventionInput>())
            {
                var entity = CareIntervention.Create(ivInput.AssignedRole);
                plan.CareInterventions.Add(entity);
                interventions.Add((entity, ivInput));
            }

            built.Add(new BuiltArea(area, goal, interventions));
        }

        return built;
    }

    private static CarePlanDetailDto MapToDetail(CarePlan plan, List<BuiltArea> built)
    {
        var areas = built.Select(b => new CareAreaResultDto(
            GoalId: b.Goal.Id,
            GoalStatus: b.Goal.Status,
            Name: b.Area.Name,
            Source: b.Area.Source,
            Goal: b.Area.Goal,
            Measure: b.Area.Measure,
            TargetDate: b.Area.TargetDate,
            Interventions: b.Interventions.Select(x => new InterventionResultDto(
                InterventionId: x.Entity.Id,
                AssignedRole: x.Entity.AssignedRole,
                Description: x.Input.Description,
                TaskType: x.Input.TaskType,
                Frequency: x.Input.Frequency
            )).ToList()
        )).ToList();

        return new CarePlanDetailDto(
            Id: plan.Id,
            ResidentId: plan.ResidentId,
            Status: plan.Status,
            StatusFlow: "Draft → Pending Review → Active",
            SignificantChangeFlag: plan.SignificantChangeFlag,
            CareAreas: areas);
    }

    private sealed record BuiltArea(
        CareAreaInput Area,
        CareGoal Goal,
        List<(CareIntervention Entity, CarePlanInterventionInput Input)> Interventions);
}
