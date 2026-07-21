using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Features.CarePlanAcknowledgments.DTOs;
using NursingHome.Infrastructure.Persistence.DbContexts;

namespace NursingHome.Infrastructure.Persistence.Repositories.CarePlanAcknowledgments;

public class CarePlanAcknowledgmentRepository : ICarePlanAcknowledgmentRepository
{
    private readonly NursingHomeDbContext _dbContext;

    public CarePlanAcknowledgmentRepository(NursingHomeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CarePlanAcknowledgmentCoreDto?> GetCarePlanAcknowledgmentCoreAsync(
        long carePlanId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.CarePlans
            .AsNoTracking()
            .Where(carePlan => carePlan.Id == carePlanId)
            .Select(carePlan => new CarePlanAcknowledgmentCoreDto
            {
                CarePlan = new CarePlanAcknowledgmentSummaryDto
                {
                    CarePlanId = carePlan.Id,
                    Status = carePlan.Status,
                    SignificantChangeFlag = carePlan.SignificantChangeFlag,
                    CreatedAt = carePlan.CreatedAt,
                    UpdatedAt = carePlan.UpdatedAt
                },
                Resident = new CarePlanAcknowledgmentResidentDto
                {
                    ResidentId = carePlan.Resident.Id,
                    FullName = carePlan.Resident.MiddleName == null ||
                               carePlan.Resident.MiddleName == ""
                        ? carePlan.Resident.FirstName + " " + carePlan.Resident.LastName
                        : carePlan.Resident.FirstName + " " +
                          carePlan.Resident.MiddleName + " " +
                          carePlan.Resident.LastName
                },
                Goals = carePlan.CareGoals
                    .OrderBy(goal => goal.Id)
                    .Select(goal => new CarePlanGoalSummaryDto
                    {
                        GoalId = goal.Id,
                        Status = goal.Status
                    })
                    .ToList(),
                Interventions = carePlan.CareInterventions
                    .OrderBy(intervention => intervention.Id)
                    .Select(intervention => new CarePlanInterventionSummaryDto
                    {
                        InterventionId = intervention.Id,
                        AssignedRole = intervention.AssignedRole
                    })
                    .ToList(),
                Tasks = carePlan.CareInterventions
                    .SelectMany(intervention => intervention.CareTasks)
                    .OrderBy(task => task.ScheduledTime)
                    .ThenBy(task => task.Id)
                    .Select(task => new CarePlanTaskSummaryDto
                    {
                        TaskId = task.Id,
                        TaskType = task.TaskType,
                        Status = task.Status,
                        ScheduledTime = task.ScheduledTime
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<CarePlanCurrentUserDto?> GetCurrentUserAsync(
        long currentUserId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .Where(user => user.Id == currentUserId)
            .Select(user => new CarePlanCurrentUserDto
            {
                UserId = user.Id,
                FullName = user.MiddleName == null ||
                           user.MiddleName == ""
                    ? user.FirstName + " " + user.LastName
                    : user.FirstName + " " + user.MiddleName + " " + user.LastName,
                RoleId = user.RoleId,
                RoleName = user.Role.RoleName,
                LicenseNumber = user.LicenseNumber
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
