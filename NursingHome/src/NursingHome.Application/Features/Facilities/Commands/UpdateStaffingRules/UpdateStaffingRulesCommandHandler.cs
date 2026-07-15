using MediatR;
using NursingHome.Application.Abstractions.Services;
using NursingHome.Application.Features.Facilities.DTOs.Facility;

namespace NursingHome.Application.Features.Facilities.Commands.UpdateStaffingRules;

public class UpdateStaffingRulesCommandHandler : IRequestHandler<UpdateStaffingRulesCommand, Unit>
{
    private readonly IStaffingRuleService _ruleService;

    public UpdateStaffingRulesCommandHandler(IStaffingRuleService ruleService)
    {
        _ruleService = ruleService;
    }

    public async Task<Unit> Handle(UpdateStaffingRulesCommand request, CancellationToken cancellationToken)
    {
        var configDto = new StaffingRuleConfigDto
        {
            Standard = request.Standard,
            Emergency = request.Emergency
        };

        await _ruleService.UpdateRulesAsync(request.FacilityId, configDto, request.PerformedByUserId, cancellationToken);
        return Unit.Value;
    }
}
