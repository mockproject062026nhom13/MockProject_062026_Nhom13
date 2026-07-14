using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Abstractions;
using NursingHome.Domain.Entities;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Infrastructure.Persistence.Mappers;

namespace NursingHome.Infrastructure.Persistence.Repositories.LocationInfrastructure;

public class LOCRateRepository : ILOCRateRepository
{
    private readonly NursingHomeDbContext _context;

    public LOCRateRepository(NursingHomeDbContext context)
    {
        _context = context;
    }
    public async Task<List<CareLevelRate>> GetLOCRatesAsync(
        DateOnly? fromDate = null,
        DateOnly? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.CareLevelRates
            .AsNoTracking();

        if (fromDate.HasValue && toDate.HasValue)
        {
            query = query.Where(x =>
                x.EffectiveFrom <= toDate.Value &&
                (x.EffectiveTo == null || x.EffectiveTo >= fromDate.Value));
        }

        var entities = await query
            .ToListAsync(cancellationToken);

        return entities
            .Select(x => x.ToDomain())
            .ToList();
    }
    public async Task<CareLevelRate?> GetLOCRateByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.CareLevelRates
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity?.ToDomain();
    }
    public async Task AddAsync(
        CareLevelRate careLevelRate,
        CancellationToken cancellationToken = default)
    {
        var entity = Generated.CareLevelRate.Create(
            careLevelRate.CareLevelId,
            careLevelRate.FacilityId,
            careLevelRate.DailyRate,
            careLevelRate.EffectiveFrom,
            careLevelRate.EffectiveTo);

        await _context.CareLevelRates.AddAsync(entity, cancellationToken);
    }

    public async Task UpdateAsync(
        CareLevelRate careLevelRate,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.CareLevelRates
            .FirstOrDefaultAsync(
                x => x.Id == careLevelRate.Id,
                cancellationToken);

        if (entity is null)
        {
            throw new KeyNotFoundException(
                $"CareLevelRate {careLevelRate.Id} not found.");
        }

        entity.UpdatePersistence(careLevelRate);
    }

    public async Task<bool> ExistsOverlapAsync(
        long facilityId,
        long careLevelId,
        DateOnly effectiveFrom,
        DateOnly? effectiveTo,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.CareLevelRates
            .Where(x =>
                x.FacilityId == facilityId &&
                x.CareLevelId == careLevelId);

        if (excludeId.HasValue)
        {
            query = query.Where(x => x.Id != excludeId.Value);
        }

        return await query.AnyAsync(x =>
            x.EffectiveFrom <= (effectiveTo ?? DateOnly.MaxValue) &&
            (x.EffectiveTo ?? DateOnly.MaxValue) >= effectiveFrom,
            cancellationToken);
    }
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}