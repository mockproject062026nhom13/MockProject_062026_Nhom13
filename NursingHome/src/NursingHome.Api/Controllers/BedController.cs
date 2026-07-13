using MediatR;
using Microsoft.AspNetCore.Mvc;
using NursingHome.Application.Common;
using NursingHome.Application.Features.Beds.GetBeds;
using NursingHome.Domain.Constant;

namespace NursingHome.API.Controllers;

[Route("api/roomtypes")]
public class RoomTypeController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var response = ApiResponse<List<string>>.CreateSuccess(
            RoomConstants.RoomTypes.ToList());

        return Ok(response);
    }
}

[Route("api/bed")]
public class BedController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int page = 1, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetBedsQuery(page), ct);
        return Ok(result);
    }
}
