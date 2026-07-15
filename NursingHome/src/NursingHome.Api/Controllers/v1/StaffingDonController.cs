using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NursingHome.Application.Common;
using NursingHome.Application.Features.Facilities.Queries.GetDonCompliance;
using NursingHome.Application.Features.Facilities.DTOs.Facility;

namespace NursingHome.Api.Controllers.v1;

[ApiController]
[Authorize(Roles = "DON - Director of Nursing,System_Administrator")]
public class StaffingDonController : ControllerBase
{
    private readonly ISender _sender;

    public StaffingDonController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("api/don/compliance/{facilityId}")]
    public async Task<IActionResult> GetDonCompliance(long facilityId, [FromQuery] DateOnly? date, CancellationToken cancellationToken)
    {
        var query = new GetDonComplianceQuery(facilityId, date);
        var response = await _sender.Send(query, cancellationToken);
        return Ok(ApiResponse<ComplianceResultDto>.CreateSuccess(response));
    }
}
