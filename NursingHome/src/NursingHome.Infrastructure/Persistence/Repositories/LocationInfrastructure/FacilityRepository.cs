using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Features.LocationInfrastructure;
using NursingHome.Application.Abstractions;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Application.Features.Facilities.DTOs.Facility;

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
    public async Task<List<FacilityResidentStatisticDto>> GetResidentStatisticsByFacilityAsync(
        CancellationToken cancellationToken)
    {
        var data = await db.Residents
            .Where(r => r.Bed != null)
            .Select(r => new
            {
                FacilityId = r.Bed!.Room.FacilityId,

                LevelOfCareId = r.ResidentCareLevelHistories
                    .OrderByDescending(x => x.StartDate)
                    .Select(x => (long?)x.CareLevel.Id)
                    .FirstOrDefault()
            })
            .GroupBy(x => new
            {
                x.FacilityId,
                x.LevelOfCareId
            })
            .Select(g => new
            {
                g.Key.FacilityId,
                g.Key.LevelOfCareId,
                Total = g.Count()
            })
            .ToListAsync(cancellationToken);

        return data
            .GroupBy(x => x.FacilityId)
            .Select(g => new FacilityResidentStatisticDto
            {
                FacilityId = g.Key,
                Levels = g.Select(x => new LevelOfCareStatisticDto
                {
                    LevelOfCareId = x.LevelOfCareId,
                    Total = x.Total
                }).ToList()
            })
            .ToList();
    }


}




