using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NursingHome.Application.Common;
using NursingHome.Application.Features.Facilities.Commands.UpdateStaffingRules;
using NursingHome.Application.Features.Facilities.Commands.ToggleEmergencyMode;
using NursingHome.Application.Features.Facilities.Queries.GetStaffingRules;
using NursingHome.Application.Features.Facilities.DTOs.Facility;
using System.IdentityModel.Tokens.Jwt;

namespace NursingHome.Api.Controllers.v1;

[ApiController]
[Authorize(Roles = "System_Administrator")]
public class StaffingAdminController : ControllerBase
{
    private readonly ISender _sender;

    public StaffingAdminController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("api/admin/staffing-rules/{facilityId}")]
    public async Task<IActionResult> GetStaffingRules(long facilityId, CancellationToken cancellationToken)
    {
        var query = new GetStaffingRulesQuery(facilityId);
        var response = await _sender.Send(query, cancellationToken);
        return Ok(ApiResponse<StaffingRuleConfigDto>.CreateSuccess(response));
    }

    [HttpPut("api/admin/staffing-rules/{facilityId}")]
    public async Task<IActionResult> UpdateStaffingRules(long facilityId, [FromBody] UpdateStaffingRulesCommand command, CancellationToken cancellationToken)
    {
        command.FacilityId = facilityId;
        
        var subClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (long.TryParse(subClaim, out long userId))
        {
            command.PerformedByUserId = userId;
        }

        await _sender.Send(command, cancellationToken);
        return Ok(ApiResponse<string>.CreateSuccess("Staffing ratio rules updated successfully."));
    }

    [HttpPost("api/admin/facilities/{facilityId}/emergency-mode")]
    public async Task<IActionResult> ToggleEmergencyMode(long facilityId, CancellationToken cancellationToken)
    {
        var command = new ToggleEmergencyModeCommand { FacilityId = facilityId };
        
        var subClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (long.TryParse(subClaim, out long userId))
        {
            command.PerformedByUserId = userId;
        }

        var response = await _sender.Send(command, cancellationToken);
        return Ok(ApiResponse<StaffingRuleConfigDto>.CreateSuccess(response));
    }
}
