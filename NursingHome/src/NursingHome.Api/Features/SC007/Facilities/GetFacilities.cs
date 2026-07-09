using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Application.Common;

public record FacilityResponse(
    string FacilityName,
    string FacilityCode,
    string LicenseNumber,
    string TargetState,
    string City);

public class GetFacilityEndpoint(NursingHomeDbContext db) :
  EndpointWithoutRequest<ApiResponse<List<FacilityResponse>>>
{
    public override void Configure()
    {
        Get("api/facilities/info");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await db.Facilities
            .Join(db.Addresses,
                f => f.AddressId,
                a => a.Id,
                (f, a) => new FacilityResponse(
                    f.Name,
                    f.FacilityCode,
                    f.LicenseNumber,
                    f.TargetState,
                    a.City
                ))
            .ToListAsync(ct);
        var response = ApiResponse<List<FacilityResponse>>.CreateSuccess(result);
        await SendAsync(response, cancellation: ct);
    }
}
