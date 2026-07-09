using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Common;
using NursingHome.Infrastructure.Persistence.DbContexts;

public record AssessmentByResidentRecord(
    string ResidentFullName,
    string UserFullName,
    int Score,
    long ConfirmedCareLevel,
    DateTime Date
);

public record GetAssessmentsByResidentRequest(long ResidentId);

public class GetAssessmentsByResidentEndpoint(NursingHomeDbContext db) : Endpoint<GetAssessmentsByResidentRequest, ApiResponse<List<AssessmentByResidentRecord>>>
{
    public override void Configure()
    {
        Get("/api/residents/{ResidentId}/assessments");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetAssessmentsByResidentRequest req, CancellationToken ct)
    {
        var data = await db.Assessments
            .Where(a => a.ResidentId == req.ResidentId)
            .OrderByDescending(a => a.CreatedAt)
            .Select(a => new AssessmentByResidentRecord(
                $"{a.Resident.FirstName} {a.Resident.LastName}",
                $"{a.AssessedByNavigation.FirstName} {a.AssessedByNavigation.LastName}",
                a.AssessmentDetails.Max(d => d.Score),
                a.ConfirmedCareLevelId,
                a.CreatedAt.DateTime
            ))
            .ToListAsync(ct);

        await SendAsync(ApiResponse<List<AssessmentByResidentRecord>>.CreateSuccess(data), cancellation: ct);
    }
}
