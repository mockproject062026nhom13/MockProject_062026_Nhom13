using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Features.LocationInfrastructure;
using NursingHome.Application.Abstractions;
using NursingHome.Infrastructure.Persistence.DbContexts;

namespace NursingHome.Infrastructure.Persistence.Repositories;

public class FacilityRepository(NursingHomeDbContext db) : IFacilityRepository
{
    public async Task<List<FacilityResponse>> GetFacilitiesAsync(CancellationToken ct)
    {
        return await db.Facilities
            .Join(
                db.Addresses,
                f => f.AddressId,
                a => a.Id,
                (f, a) => new FacilityResponse(
                    f.Name,
                    f.FacilityCode,
                    f.LicenseNumber,
                    f.TargetState,
                    a.City))
            .ToListAsync(ct);
    }
}




