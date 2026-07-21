using MediatR;
using NursingHome.Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;
<<<<<<< HEAD
using NursingHome.Domain.Constants;

=======
using  NursingHome.Application.Features.Facilities.GetResidentStatisticsByFacility.Queries;
using NursingHome.Domain.Constants;
using NursingHome.Infrastructure.Authorization;
>>>>>>> origin/dev
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
    //CarePlanView
    [HttpGet("facility-resident-statistics")]
    [PermissionAuthorize(PermissionConstants.CarePlanView)]
    public async Task<IActionResult> GetResidentStatisticsAsync()
    {
        var query = new GetResidentStatisticsByFacilityQuery();

        var result = await mediator.Send(query);

        return StatusCode(result.StatusCode, result);
    }

}
