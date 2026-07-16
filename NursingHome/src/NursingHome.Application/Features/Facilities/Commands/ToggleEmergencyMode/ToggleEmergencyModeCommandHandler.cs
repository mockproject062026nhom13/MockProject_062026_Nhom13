using MediatR;
using NursingHome.Application.Abstractions.Services;
using NursingHome.Application.Features.Facilities.DTOs.Facility;

namespace NursingHome.Application.Features.Facilities.Commands.ToggleEmergencyMode;

public class ToggleEmergencyModeCommandHandler : IRequestHandler<ToggleEmergencyModeCommand, StaffingRuleConfigDto>
{
    private readonly IStaffingRuleService _ruleService;

    public ToggleEmergencyModeCommandHandler(IStaffingRuleService ruleService)
    {
        _ruleService = ruleService;
    }

    public async Task<StaffingRuleConfigDto> Handle(ToggleEmergencyModeCommand request, CancellationToken cancellationToken)
    {
        return await _ruleService.ToggleEmergencyModeAsync(request.FacilityId, request.PerformedByUserId, cancellationToken);
    }
}
