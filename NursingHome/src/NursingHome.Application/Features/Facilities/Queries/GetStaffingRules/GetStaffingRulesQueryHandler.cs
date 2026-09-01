using MediatR;
using NursingHome.Application.Abstractions.Services;
using NursingHome.Application.Features.Facilities.DTOs.Facility;

namespace NursingHome.Application.Features.Facilities.Queries.GetStaffingRules;

public class GetStaffingRulesQueryHandler : IRequestHandler<GetStaffingRulesQuery, StaffingRuleConfigDto>
{
    private readonly IStaffingRuleService _ruleService;

    public GetStaffingRulesQueryHandler(IStaffingRuleService ruleService)
    {
        _ruleService = ruleService;
    }

    public async Task<StaffingRuleConfigDto> Handle(GetStaffingRulesQuery request, CancellationToken cancellationToken)
    {
        return await _ruleService.GetRulesAsync(request.FacilityId, cancellationToken);
    }
}
