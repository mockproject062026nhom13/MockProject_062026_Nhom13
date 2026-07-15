using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Features.CareLevelResidents.Queries;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Application.Features.CareLevelResidents.DTOs;

public class CarePlanRepository : ICarePlanRepository
{
    private readonly NursingHomeDbContext _context;

    public CarePlanRepository(NursingHomeDbContext context)
    {
        _context = context;
    }
    public async Task<CarePlanInfoDto?> GetCarePlanInfoAsync(
        long carePlanId,
        CancellationToken cancellationToken)
    {
        return await _context.CarePlans
            .Where(cp => cp.Id == carePlanId)
            .Select(cp => new CarePlanInfoDto
            {
                ResidentId = cp.Resident.Id,

                FullName =
                    cp.Resident.FirstName +
                    (cp.Resident.MiddleName != null
                        ? " " + cp.Resident.MiddleName
                        : "")
                    + " " +
                    cp.Resident.LastName,
                Status = cp.Status,

                LevelOfCare = cp.Resident.ResidentCareLevelHistories
                    .OrderByDescending(x => x.StartDate)
                    .Select(x => x.CareLevel.Id)
                    .FirstOrDefault(),

                Bed = cp.Resident.Bed != null
                    ? cp.Resident.Bed.BedNumber
                    : null,

                RoomNumber = cp.Resident.Bed != null
                    ? cp.Resident.Bed.Room.RoomNumber
                    : null,

                CareTasks = cp.CareInterventions
                    .SelectMany(ci => ci.CareTasks)
                    .Where(ct => ct.AssignedCnaId != null)
                    .Select(ct => new CareTaskAssignmentDto
                    {
                        TaskType = ct.TaskType,

                        AssignedCnaId = ct.AssignedCnaId!.Value,

                        EmployeeCode = ct.AssignedCna!.EmployeeCode,

                        FullName =
                            ct.AssignedCna.FirstName +
                            (ct.AssignedCna.MiddleName != null
                                ? " " + ct.AssignedCna.MiddleName
                                : "")
                            + " " +
                            ct.AssignedCna.LastName
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<List<long>> GetCarePlanIdByStatus(
        string status,
        CancellationToken cancellationToken)
    {
        return await _context.CarePlans
            .Where(cp => cp.Status == status)
            .Select(cp => cp.Id)
            .ToListAsync(cancellationToken);
    }
    public async Task UpdateStatusByDON(
        long careplanId,
        string status,
        CancellationToken cancellationToken)
    {
        var carePlan = await _context.CarePlans
            .FirstOrDefaultAsync(
                x => x.Id == careplanId && !x.IsDeleted,
                cancellationToken);

        if (carePlan == null)
        {
            throw new KeyNotFoundException(
                $"Care plan with id {careplanId} was not found.");
        }

        carePlan.UpdateStatus(status);

        await _context.SaveChangesAsync(cancellationToken);
    }
}