using MediatR;
using NursingHome.Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;
using NursingHome.Domain.Constants;

namespace NursingHome.API.Controllers;

[ApiController]
[Route("api/facilities")]
// [PermissionAuthorize(PermissionConstants.AdFacilitySettingsManage)]
public class FacilityController(IMediator mediator) : ControllerBase
{
    [HttpGet("info")]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        var result = await mediator.Send(new GetFacilitiesQuery(), ct);

        return Ok(result);
    }
}
