using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NursingHome.Application.Common;
using NursingHome.Application.Features.Facilities.Commands.UpdateStaffingConfig;
using NursingHome.Application.Features.Facilities.Queries.GetStaffingCompliance;
using NursingHome.Application.Features.Facilities.Queries.GetStaffingConfig;
using NursingHome.Application.Models.Facility;

namespace NursingHome.Api.Controllers.v1;

[ApiController]
[Route("api/v1/facilities")]
[Authorize(Roles = "System_Administrator")]
public class FacilitiesController : ControllerBase
{
    private readonly ISender _sender;

    public FacilitiesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{facilityId}/staffing-configs")]
    public async Task<IActionResult> GetStaffingConfig(long facilityId, [FromQuery] DateOnly? date, CancellationToken cancellationToken)
    {
        var query = new GetStaffingConfigQuery(facilityId, date);
        var response = await _sender.Send(query, cancellationToken);
        return Ok(ApiResponse<StaffingConfigDto>.CreateSuccess(response));
    }

    [HttpPut("{facilityId}/staffing-configs")]
    public async Task<IActionResult> UpdateStaffingConfig(long facilityId, [FromBody] UpdateStaffingConfigCommand command, CancellationToken cancellationToken)
    {
        // Gán route param vào command
        command.FacilityId = facilityId;
        
        var response = await _sender.Send(command, cancellationToken);
        return Ok(ApiResponse<StaffingConfigDto>.CreateSuccess(response));
    }

    [HttpGet("{facilityId}/compliance")]
    public async Task<IActionResult> GetCompliance(long facilityId, [FromQuery] DateOnly? date, CancellationToken cancellationToken)
    {
        var query = new NursingHome.Application.Features.Facilities.Queries.GetStaffingCompliance.GetStaffingComplianceQuery(facilityId, date);
        var response = await _sender.Send(query, cancellationToken);
        return Ok(ApiResponse<NursingHome.Application.Models.Facility.StaffingComplianceDto>.CreateSuccess(response));
    }
}
