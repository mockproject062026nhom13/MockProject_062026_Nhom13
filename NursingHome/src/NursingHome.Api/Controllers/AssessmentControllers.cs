using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Common;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Infrastructure.Persistence.Generated;

using NursingHome.Api.LOCRateDTOs;

namespace NursingHome.Api.Controllers;
/// <summary>
/// Controller for managing initial assessments.
/// </summary>
/// 
[ApiController]
[Route("api/auth")]
public class AssessmentController : ControllerBase
{
    private readonly NursingHomeDbContext _context;

    public AssessmentController(NursingHomeDbContext context)
    {
        _context = context;
    }
    // on db only have catagory and metric_name, so we can only filter by category
    [HttpGet("assessment-metrics")]
    public async Task<IActionResult> GetAssessmentMetricsAsync(
        [FromQuery] List<string>? categories)
    {
        var query = _context.AssessmentMetrics.AsQueryable();

        if (categories != null && categories.Any())
        {
            query = query.Where(x => categories.Contains(x.Category));
        }

        var assessmentMetrics = await query.ToListAsync();

        return Ok(assessmentMetrics);
    } 
    [HttpPost("assessment")]
    public async Task<IActionResult> CreateAssessmentAsync(
        [FromBody] AssessmentRequestDto request)
    {
        // ==========================
        // Validation
        // ==========================

        // TODO: Validate FacilityId exists

        // TODO: Validate ResidentId belongs to FacilityId

        // TODO: Validate UserId from JWT token
        // var userId = GetUserIdFromToken();

        // TODO: Validate AssessmentDetails is not empty

        // TODO: Validate MetricId exists

        // TODO: Validate MetricId is unique

        // TODO: Validate Value matches Metric definition

        // TODO: Validate VitalSign values (range, required fields...)

        // ==========================
        // Mock user id
        // ==========================

        var userId = 1L;

        // ==========================
        // Create Assessment
        // ==========================

        // TODO:
        // var assessment = new Assessment(...);

        // _context.Assessments.Add(assessment);
        // await _context.SaveChangesAsync();

        // Mock AssessmentId
        var assessmentId = 100L;

        // ==========================
        // Create Assessment Details
        // ==========================

        foreach (var detail in request.AssessmentDetails)
        {
            // TODO:
            // var assessmentDetail = new AssessmentDetail(
            //     assessmentId,
            //     detail.MetricId,
            //     detail.Value);

            // _context.AssessmentDetails.Add(assessmentDetail);
        }

        // ==========================
        // Create Vital Sign (optional)
        // ==========================

        if (request.VitalSign != null)
        {
            // TODO:
            // var vitalSign = new VitalSign(
            //     residentId: request.ResidentId,
            //     recordedBy: userId,
            //     bloodPressureSystolic: request.VitalSign.BloodPressureSystolic,
            //     bloodPressureDiastolic: request.VitalSign.BloodPressureDiastolic,
            //     heartRateBpm: request.VitalSign.HeartRateBpm,
            //     respiratoryRate: request.VitalSign.RespiratoryRate,
            //     temperatureFahrenheit: request.VitalSign.TemperatureFahrenheit,
            //     spo2Percentage: request.VitalSign.Spo2Percentage,
            //     painScale: request.VitalSign.PainScale,
            //     notes: request.VitalSign.Notes,
            //     recordedAt: request.VitalSign.RecordedAt ?? DateTimeOffset.UtcNow);

            // _context.VitalSigns.Add(vitalSign);
        }

        // ==========================
        // Save Changes
        // ==========================

        // await _context.SaveChangesAsync();

        return Ok(new
        {
            AssessmentId = assessmentId
        });
    }
    // todo:check permission  + facility_id + user_id + seperate architecture
    // todo: audit log for post and put
    // todo: standardization for response and error handling

    // todo: add http post to add new loc rate if nessary

}
// todo: apply ApiResponse and ApiError to the response of this controller
record AssessmentRequestDto
{
    long FacilityId { get; init; }

    long UserId { get; init; }

    long ResidentId { get; init; }

    List<AssessmentDetailDto> AssessmentDetails { get; init; } = new();

    VitalSignDto VitalSign { get; init; } = new();

    long VitalSignById { get; init; }
}

record AssessmentDetailDto
{
    long MetricId { get; init; }

    string Value { get; init; } = string.Empty;
}

record VitalSignDto
{
    short BloodPressureSystolic { get; init; }

    short BloodPressureDiastolic { get; init; }

    short HeartRateBpm { get; init; }

    short RespiratoryRate { get; init; }

    decimal TemperatureFahrenheit { get; init; }

    byte Spo2Percentage { get; init; }

    byte PainScale { get; init; }

    string Notes { get; init; } = string.Empty;

    DateTimeOffset RecordedAt { get; init; }
}



