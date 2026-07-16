using MediatR;
using Microsoft.AspNetCore.Mvc;
using  NursingHome.Application.Features.Facilities.GetResidentStatisticsByFacility.Queries;
namespace NursingHome.API.Controllers;

[ApiController]
[Route("api/facilities")]
public class FacilityController(IMediator mediator) : ControllerBase
{
    [HttpGet("info")]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        var result = await mediator.Send(new GetFacilitiesQuery(), ct);

        return Ok(result);
    }

    [HttpGet("facility-resident-statistics")]
    public async Task<IActionResult> GetResidentStatisticsAsync()
    {
        var query = new GetResidentStatisticsByFacilityQuery();

        var result = await mediator.Send(query);

        return StatusCode(result.StatusCode, result);
    }

}
