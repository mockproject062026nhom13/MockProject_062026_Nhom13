using MediatR;
using Microsoft.AspNetCore.Mvc;

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
}
