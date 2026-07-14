using NursingHome.Application.Features.Facilities.DTOs.Facility;

namespace NursingHome.Application.Abstractions.Repositories;

public interface IStaffingComplianceRepository
{
    Task<StaffingComplianceDto> GetComplianceAsync(long facilityId, DateOnly date, CancellationToken cancellationToken = default);
}
