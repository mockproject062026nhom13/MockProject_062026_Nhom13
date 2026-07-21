using MediatR;
using Microsoft.AspNetCore.Mvc;
using NursingHome.Application.Common;
using NursingHome.Application.Features.Beds.GetBeds;
using NursingHome.Domain.Constant;

namespace NursingHome.API.Controllers;

[Route("api/roomtype")]
public class RoomTypeController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var roomTypesWithPrice = RoomConstants.RoomTypes.Select(type => new 
        {
            Name = type,
            Price = type switch
            {
                "private" => 150L,
                "semi-private" => 100L,
                _ => 50L 
            }
        }).ToList();

        var response = ApiResponse<object>.CreateSuccess(roomTypesWithPrice);

        return Ok(response);
    }
    }


[Route("api/beds")]
public class BedController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int page = 1, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetBedsQuery(page), ct);
        return Ok(result);
    }
}
