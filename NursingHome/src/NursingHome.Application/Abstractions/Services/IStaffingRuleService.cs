using NursingHome.Application.Features.Facilities.DTOs.Facility;

namespace NursingHome.Application.Abstractions.Services;

public interface IStaffingRuleService
{
    Task<StaffingRuleConfigDto> GetRulesAsync(long facilityId, CancellationToken cancellationToken = default);
    Task UpdateRulesAsync(long facilityId, StaffingRuleConfigDto config, long performedByUserId, CancellationToken cancellationToken = default);
    Task<StaffingRuleConfigDto> ToggleEmergencyModeAsync(long facilityId, long performedByUserId, CancellationToken cancellationToken = default);
    Task<ComplianceResultDto> GetComplianceAsync(long facilityId, DateOnly date, CancellationToken cancellationToken = default);
}
