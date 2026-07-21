using FastEndpoints;
using NursingHome.Application.Features.CareLevelResidents.DTOs;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Application.Common;

public class GetAssessmentsEndpoint(NursingHomeDbContext db)
    : Endpoint<GetAssessmentsCommand, ApiResponse<IEnumerable<ResidentAssessmentDto>>>
{
    public override void Configure()
    {
        Get("/api/assessments/{UserId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetAssessmentsCommand req, CancellationToken ct)
    {
        var result = await new GetAssessmentsCommandHandler(db).ExecuteAsync(req, ct);
        var response = ApiResponse<IEnumerable<ResidentAssessmentDto>>.CreateSuccess(result);
        await SendOkAsync(response, ct);
    }
}
