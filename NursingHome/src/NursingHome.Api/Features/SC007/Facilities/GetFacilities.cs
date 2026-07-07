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
        var result = await db.facilities
            .Join(db.addresses,
                f => f.address_id,
                a => a.id,
                (f, a) => new FacilityResponse(
                    f.name,
                    f.facility_code,
                    f.license_number,
                    f.target_state,
                    a.city
                ))
            .ToListAsync(ct);
        var response = ApiResponse<List<FacilityResponse>>.CreateSuccess(result);
        await SendAsync(response, cancellation: ct);
    }
}
