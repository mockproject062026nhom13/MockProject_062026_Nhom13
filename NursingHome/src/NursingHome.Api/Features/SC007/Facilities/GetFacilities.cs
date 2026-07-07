using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using NursingHome.Infrastructure.Persistence.DbContexts;

public record FacilityResponse(
    string FacilityName,
    string FacilityCode,
    string LicenseNumber,
    string TargetState,
    string City);

public class GetFacilityEndpoint(NursingHomeDbContext db) : EndpointWithoutRequest<List<FacilityResponse>>
{
    public override void Configure()
    {
        Get("/facilities/info");
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

        await SendAsync(result, cancellation: ct);
    }
}
