using MediatR;
using Microsoft.AspNetCore.Mvc;
using NursingHome.Application.Features.Facilities.Commands.UpdateStaffingConfig;
using NursingHome.Application.Features.Facilities.Queries.GetStaffingConfig;

namespace NursingHome.Api.Controllers.v1;

[ApiController]
[Route("api/v1/facilities")]
public class FacilitiesController : ControllerBase
{
    private readonly ISender _sender;

    public FacilitiesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{facilityId}/staffing-configs")]
    public async Task<IActionResult> GetStaffingConfig(long facilityId, CancellationToken cancellationToken)
    {
        var query = new GetStaffingConfigQuery(facilityId);
        var response = await _sender.Send(query, cancellationToken);
        return Ok(response);
    }

    [HttpPut("{facilityId}/staffing-configs")]
    public async Task<IActionResult> UpdateStaffingConfig(long facilityId, [FromBody] UpdateStaffingConfigCommand command, CancellationToken cancellationToken)
    {
        // Gán route param vào command
        command.FacilityId = facilityId;
        
        var response = await _sender.Send(command, cancellationToken);
        return Ok(response);
    }
}
