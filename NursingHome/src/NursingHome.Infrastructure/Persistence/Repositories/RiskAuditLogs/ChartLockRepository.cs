using System.Data;
using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Features.RiskAuditLogs.DTOs;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Infrastructure.Persistence.Generated;

namespace NursingHome.Infrastructure.Persistence.Repositories.RiskAuditLogs;

public sealed class ChartLockRepository : IChartLockRepository
{
    private const string ChartUnlockedAction = "CHART_UNLOCKED";

    private readonly NursingHomeDbContext _context;

    public ChartLockRepository(NursingHomeDbContext context)
    {
        _context = context;
    }

    public async Task<IncidentChartLockDetailDto?> GetIncidentChartLockAsync(
        long incidentId,
        CancellationToken cancellationToken)
    {
        return await _context.Incidents
            .AsNoTracking()
            .Where(incident => incident.Id == incidentId)
            .Select(incident => new IncidentChartLockDetailDto
            {
                Incident = new IncidentChartLockIncidentDto
                {
                    IncidentId = incident.Id,
                    Status = incident.Status,
                    IncidentType = incident.IncidentType,
                    ReportedAt = incident.ReportedAt,
                    Description = incident.Description
                },
                Severity = new IncidentChartLockSeverityDto
                {
                    SeverityId = incident.Severity.Id,
                    LevelName = incident.Severity.LevelName,
                    ChartLockTrigger = incident.Severity.ChartLockTrigger
                },
                Resident = new IncidentChartLockResidentDto
                {
                    ResidentId = incident.Resident.Id,
                    FullName = incident.Resident.MiddleName == null ||
                               incident.Resident.MiddleName == ""
                        ? incident.Resident.FirstName + " " + incident.Resident.LastName
                        : incident.Resident.FirstName + " " +
                          incident.Resident.MiddleName + " " +
                          incident.Resident.LastName,
                    IsChartLocked = incident.Resident.IsChartLocked
                },
                Timeline = incident.IncidentTimelines
                    .OrderByDescending(timeline => timeline.CreatedAt)
                    .ThenByDescending(timeline => timeline.Id)
                    .Select(timeline => new IncidentChartLockTimelineItemDto
                    {
                        TimelineId = timeline.Id,
                        Action = timeline.Action,
                        Reason = timeline.Reason,
                        ActorId = timeline.Actor,
                        CreatedAt = timeline.CreatedAt
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ChartUnlockOperationResult> UnlockResidentChartAsync(
        long incidentId,
        long currentUserId,
        string reason,
        CancellationToken cancellationToken)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(
            IsolationLevel.Serializable,
            cancellationToken);

        var incidentInfo = await _context.Incidents
            .AsNoTracking()
            .Where(incident => incident.Id == incidentId)
            .Select(incident => new
            {
                IncidentId = incident.Id,
                incident.ResidentId
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (incidentInfo is null)
        {
            return ChartUnlockOperationResult.NotFound();
        }

        var resident = await _context.Residents
            .FromSqlInterpolated(
                $"SELECT * FROM residents WITH (UPDLOCK, ROWLOCK) WHERE id = {incidentInfo.ResidentId}")
            .FirstOrDefaultAsync(cancellationToken);

        if (resident is null)
        {
            return ChartUnlockOperationResult.NotFound();
        }

        if (!resident.IsChartLocked)
        {
            return ChartUnlockOperationResult.AlreadyUnlocked();
        }

        var unlockedAt = DateTimeOffset.UtcNow;

        _context.Entry(resident)
            .Property(nameof(Resident.IsChartLocked))
            .CurrentValue = false;

        _context.Entry(resident)
            .Property(nameof(Resident.IsChartLocked))
            .IsModified = true;

        var timeline = IncidentTimeline.Create(
            incidentId: incidentInfo.IncidentId,
            action: ChartUnlockedAction,
            reason: reason,
            actor: currentUserId,
            createdAt: unlockedAt);

        _context.IncidentTimelines.Add(timeline);

        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return ChartUnlockOperationResult.Success(
            new UnlockResidentChartResultDto
            {
                IncidentId = incidentInfo.IncidentId,
                ResidentId = incidentInfo.ResidentId,
                IsChartLocked = false,
                UnlockedAt = unlockedAt,
                UnlockedByUserId = currentUserId,
                TimelineId = timeline.Id
            });
    }
}
