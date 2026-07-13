using NursingHome.Application.Models.Facility;

namespace NursingHome.Application.Abstractions.Repositories;

public interface IStaffingConfigRepository
{
    Task<StaffingConfigDto?> GetByFacilityIdAsync(long facilityId, CancellationToken cancellationToken = default);
    Task<StaffingConfigDto> AddOrUpdateAsync(long facilityId, decimal minHrs, int warnPct, CancellationToken cancellationToken = default);
}
