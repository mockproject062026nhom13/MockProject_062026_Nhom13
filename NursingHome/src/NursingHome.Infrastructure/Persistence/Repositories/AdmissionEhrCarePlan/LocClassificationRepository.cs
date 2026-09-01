using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Features.AdmissionEhrCarePlan.DTOs;
using NursingHome.Infrastructure.Persistence.DbContexts;

namespace NursingHome.Infrastructure.Persistence.Repositories.AdmissionEhrCarePlan;

public class LocClassificationRepository : ILocClassificationRepository
{
    private readonly NursingHomeDbContext _dbContext;

    public LocClassificationRepository(NursingHomeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<LocClassificationResultDto?> GetLocClassificationResultAsync(
        long assessmentId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Assessments
            .AsNoTracking()
            .Where(assessment => assessment.Id == assessmentId)
            .Select(assessment => new LocClassificationResultDto
            {
                Assessment = new LocAssessmentSummaryDto
                {
                    AssessmentId = assessment.Id,
                    CreatedAt = assessment.CreatedAt,
                    AssessedBy = new AssessedByDto
                    {
                        UserId = assessment.AssessedByNavigation.Id,
                        FullName = assessment.AssessedByNavigation.MiddleName == null ||
                                   assessment.AssessedByNavigation.MiddleName == ""
                            ? assessment.AssessedByNavigation.FirstName + " " +
                              assessment.AssessedByNavigation.LastName
                            : assessment.AssessedByNavigation.FirstName + " " +
                              assessment.AssessedByNavigation.MiddleName + " " +
                              assessment.AssessedByNavigation.LastName,
                        RoleId = assessment.AssessedByNavigation.RoleId,
                        RoleName = assessment.AssessedByNavigation.Role.RoleName
                    }
                },
                Resident = new LocResidentSummaryDto
                {
                    ResidentId = assessment.Resident.Id,
                    FullName = assessment.Resident.MiddleName == null ||
                               assessment.Resident.MiddleName == ""
                        ? assessment.Resident.FirstName + " " + assessment.Resident.LastName
                        : assessment.Resident.FirstName + " " +
                          assessment.Resident.MiddleName + " " +
                          assessment.Resident.LastName
                },
                AdlSummary = new AdlSummaryDto
                {
                    AdlTotalScore = assessment.AdlTotalScore
                },
                AdlItems = assessment.AssessmentDetails
                    .Where(detail => detail.Metric.Category == "ADL")
                    .OrderBy(detail => detail.MetricId)
                    .Select(detail => new AdlItemDto
                    {
                        MetricId = detail.MetricId,
                        MetricName = detail.Metric.MetricName,
                        Category = detail.Metric.Category,
                        Score = detail.Score,
                        Notes = detail.Notes
                    })
                    .ToList(),
                SuggestedLoc = new CareLevelSummaryDto
                {
                    CareLevelId = assessment.SuggestedCareLevel.Id,
                    LevelCode = assessment.SuggestedCareLevel.LevelCode,
                    LevelName = assessment.SuggestedCareLevel.LevelName
                },
                ConfirmedLoc = new CareLevelSummaryDto
                {
                    CareLevelId = assessment.ConfirmedCareLevel.Id,
                    LevelCode = assessment.ConfirmedCareLevel.LevelCode,
                    LevelName = assessment.ConfirmedCareLevel.LevelName
                },
                IsOverridden = assessment.IsOverridden
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<LocHistoryResultDto?> GetLocHistoryAsync(
        long residentId,
        CancellationToken cancellationToken)
    {
        var resident = await _dbContext.Residents
            .AsNoTracking()
            .Where(resident => resident.Id == residentId)
            .Select(resident => new LocHistoryResidentDto
            {
                ResidentId = resident.Id,
                FullName = resident.MiddleName == null ||
                           resident.MiddleName == ""
                    ? resident.FirstName + " " + resident.LastName
                    : resident.FirstName + " " +
                      resident.MiddleName + " " +
                      resident.LastName
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (resident is null)
        {
            return null;
        }

        var historyItems = await _dbContext.ResidentCareLevelHistories
            .AsNoTracking()
            .Where(history => history.ResidentId == residentId)
            .OrderByDescending(history => history.StartDate)
            .ThenByDescending(history => history.Id)
            .Select(history => new LocHistoryItemDto
            {
                HistoryId = history.Id,
                CareLevelId = history.CareLevelId,
                LevelCode = history.CareLevel.LevelCode,
                LevelName = history.CareLevel.LevelName,
                StartDate = history.StartDate,
                EndDate = history.EndDate,
                IsCurrent = history.EndDate == null
            })
            .ToListAsync(cancellationToken);

        return new LocHistoryResultDto
        {
            Resident = resident,
            HistoryItems = historyItems
        };
    }
}
