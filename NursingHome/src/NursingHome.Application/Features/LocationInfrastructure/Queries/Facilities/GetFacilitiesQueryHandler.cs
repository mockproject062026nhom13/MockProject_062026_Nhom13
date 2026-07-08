using NursingHome.Application.Features.LocationInfrastructure;

public class GetFacilitiesQueryHandler(NursingHomeDbContext db) : IRequestHandler<GetFacilitiesQuery, List<FacilityResponse>>
{
    public async Task<List<FacilityResponse>> Handle(GetFacilitiesQuery request, CancellationToken ct)
    {
        return await db.facilities
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
    }
}
