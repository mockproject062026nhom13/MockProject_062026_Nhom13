using MediatR;
using NursingHome.Application.Abstractions.Services;
using NursingHome.Application.Features.Facilities.DTOs.Facility;

namespace NursingHome.Application.Features.Facilities.Queries.GetDonCompliance;

public class GetDonComplianceQueryHandler : IRequestHandler<GetDonComplianceQuery, ComplianceResultDto>
{
    private readonly IStaffingRuleService _ruleService;

    public GetDonComplianceQueryHandler(IStaffingRuleService ruleService)
    {
        _ruleService = ruleService;
    }

    public async Task<ComplianceResultDto> Handle(GetDonComplianceQuery request, CancellationToken cancellationToken)
    {
        var targetDate = request.Date ?? DateOnly.FromDateTime(DateTime.Today);
        return await _ruleService.GetComplianceAsync(request.FacilityId, targetDate, cancellationToken);
    }
}
