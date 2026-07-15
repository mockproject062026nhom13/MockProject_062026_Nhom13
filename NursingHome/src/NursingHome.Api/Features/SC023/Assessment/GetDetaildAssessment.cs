using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Common;
using NursingHome.Infrastructure.Persistence.DbContexts;

public record AssessmentDetailRecord(
    string MetricName,
    int Score,
    string? Notes
);

public record GetAssessmentDetailsRequest(long AssessmentId);

public class GetAssessmentDetailsEndpoint(NursingHomeDbContext db) : Endpoint<GetAssessmentDetailsRequest, ApiResponse<List<AssessmentDetailRecord>>>
{
    public override void Configure()
    {
        Get("/api/assessments/{AssessmentId}/details");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetAssessmentDetailsRequest req, CancellationToken ct)
    {
        var data = await db.AssessmentDetails
            .Where(d => d.AssessmentId == req.AssessmentId)
            .Select(d => new AssessmentDetailRecord(
                d.Metric.MetricName,
                d.Score,
                d.Notes
            ))
            .ToListAsync(ct);

        await SendAsync(ApiResponse<List<AssessmentDetailRecord>>.CreateSuccess(data), cancellation: ct);
    }
}
