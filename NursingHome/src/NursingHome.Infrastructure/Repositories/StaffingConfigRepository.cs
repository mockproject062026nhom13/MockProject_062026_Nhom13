using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Abstractions.Repositories;
using NursingHome.Application.Models.Facility;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Infrastructure.Persistence.Generated;

namespace NursingHome.Infrastructure.Repositories;

public class StaffingConfigRepository : IStaffingConfigRepository
{
    private readonly NursingHomeDbContext _dbContext;

    public StaffingConfigRepository(NursingHomeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<StaffingConfigDto?> GetByFacilityIdAsync(long facilityId, CancellationToken cancellationToken = default)
    {
        var config = await _dbContext.StaffingConfigs
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.FacilityId == facilityId, cancellationToken);

        if (config == null) return null;

        return new StaffingConfigDto
        {
            Id = config.Id,
            FacilityId = config.FacilityId,
            MinHrsPerResidentDay = config.MinHrsPerResidentDay,
            WarnBelowPercentage = config.WarnBelowPercentage
        };
    }

    public async Task<StaffingConfigDto> AddOrUpdateAsync(long facilityId, decimal minHrs, int warnPct, CancellationToken cancellationToken = default)
    {
        var facilityExists = await _dbContext.Facilities.AnyAsync(f => f.Id == facilityId, cancellationToken);
        if (!facilityExists)
        {
            throw new NursingHome.Domain.Exceptions.NotFoundException("Facility", facilityId);
        }

        var config = await _dbContext.StaffingConfigs
            .FirstOrDefaultAsync(c => c.FacilityId == facilityId, cancellationToken);

        if (config == null)
        {
            config = new StaffingConfig(facilityId, minHrs, warnPct);
            _dbContext.StaffingConfigs.Add(config);
        }
        else
        {
            config.UpdateConfig(minHrs, warnPct);
            _dbContext.StaffingConfigs.Update(config);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new StaffingConfigDto
        {
            Id = config.Id,
            FacilityId = config.FacilityId,
            MinHrsPerResidentDay = config.MinHrsPerResidentDay,
            WarnBelowPercentage = config.WarnBelowPercentage
        };
    }
}
