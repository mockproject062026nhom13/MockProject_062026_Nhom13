using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Features.CareLevelResidents.DTOs;
using NursingHome.Application.Abstractions;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Infrastructure.Persistence.Generated;

namespace NursingHome.Infrastructure.Persistence.Repositories.CareLevelResidents;

public class AssessmentService : IAssessmentService
{
    private readonly NursingHomeDbContext _context;

    public AssessmentService(NursingHomeDbContext context)
    {
        _context = context;
    }
    // mock AI :)))
    public int CalculateCareLevel(int totalScore)
    {
        return totalScore switch
        {
            >= 85 => 1, // Independent / Low care
            >= 65 => 2, // Needs setup help
            >= 45 => 3, // Requires assistance
            >= 25 => 4, // Extensive assistance
            _ => 5      // Total dependence

        };
    }
    public async Task<long> CreateAssessmentAsync(
        AssessmentCreateDTO dto,
        CancellationToken cancellationToken = default)
    {
        await using var transaction =
            await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {


        var totalScore = dto.AssessmentDetails.Sum(x => x.Score);
        if (totalScore<=0)
        {
            throw new ArgumentException("Total score is equal or low than zero.");
        }
        // get carelevel by AI or something??? but i will do some logic calculator here :)))
        var CalculateCareid = CalculateCareLevel(totalScore);

        var assessment = Assessment.Create(
            residentId: dto.ResidentId,
            assessedBy: dto.AssessedBy,
            adlTotalScore: totalScore,
            isOverridden: false,
            suggestedCareLevelId: CalculateCareid,      // TODO: calculate later
            confirmedCareLevelId: CalculateCareid,      // TODO: calculate later
            createdAt: DateTimeOffset.UtcNow);

            _context.Assessments.Add(assessment);
            await _context.SaveChangesAsync(cancellationToken);

        var details = dto.AssessmentDetails.Select(x =>
            AssessmentDetail.Create(
                assessment.Id,
                x.MetricId,
                x.Score,
                x.Notes));

            _context.AssessmentDetails.AddRange(details);

        var vitalSign = VitalSign.Create(
            residentId: dto.ResidentId,
            recordedBy: dto.AssessedBy,
            bloodPressureSystolic: dto.VitalSigns.BloodPressureSystolic,
            bloodPressureDiastolic: dto.VitalSigns.BloodPressureDiastolic,
            heartRateBpm: dto.VitalSigns.HeartRateBpm,
            respiratoryRate: dto.VitalSigns.RespiratoryRate,
            temperatureFahrenheit: dto.VitalSigns.TemperatureFahrenheit,
            spo2Percentage: dto.VitalSigns.Spo2Percentage,
            painScale: dto.VitalSigns.PainScale,
            notes: dto.VitalSigns.Notes,
            recordedAt: DateTimeOffset.UtcNow);

            _context.VitalSigns.Add(vitalSign); 

            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return assessment.Id;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}