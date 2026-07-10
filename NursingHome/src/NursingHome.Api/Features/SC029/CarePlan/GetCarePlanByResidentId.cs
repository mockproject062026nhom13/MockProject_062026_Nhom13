using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Common;
using NursingHome.Infrastructure.Persistence.DbContexts;


namespace NursingHome.Api.Features.CarePlan;
public record GetRequest(
    long ResidentId,
    long FacilityId
    );

public class GetCarePlanEndpoint(NursingHomeDbContext db)
  : Endpoint<GetRequest, ApiResponse<List<CarePlanDto>>>
{
    public override void Configure()
    {
        Get("/api/care-plan");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetRequest req, CancellationToken ct)
    {
        var data = new List<CarePlanDto>();
        var response = ApiResponse<List<CarePlanDto>>.CreateSuccess(data);
        await SendAsync(response, 200, ct);
    }
}
