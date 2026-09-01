using NursingHome.Domain.Entities;

namespace NursingHome.Application.Abstractions;

public interface ILOCRateRepository
{
    Task<List<CareLevelRate>> GetLOCRatesAsync(
        DateOnly? fromDate = null,
        DateOnly? toDate = null,
        CancellationToken cancellationToken = default);

    Task<CareLevelRate?> GetLOCRateByIdAsync(
        long id,
        CancellationToken cancellationToken = default);
    // Task<List<CareLevelRate>> GetByFacilityIdAsync(
    //     long facilityId,
    //     CancellationToken cancellationToken);
    // Task<List<CareLevelRate>> GetByFacilityIdAndDateRangeAsync(
    //     long facilityId,
    //     DateOnly fromDate,
    //     DateOnly toDate,
    //     CancellationToken cancellationToken);

    Task AddAsync(
        CareLevelRate careLevelRate,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        CareLevelRate careLevelRate,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsOverlapAsync(
        long facilityId,
        long careLevelId,
        DateOnly effectiveFrom,
        DateOnly? effectiveTo,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}