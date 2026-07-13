using NursingHome.Application.Models.Facility;

namespace NursingHome.Application.Abstractions.Repositories;

public interface IStaffingComplianceRepository
{
    Task<StaffingComplianceDto> GetComplianceAsync(long facilityId, DateOnly date, CancellationToken cancellationToken = default);
}
