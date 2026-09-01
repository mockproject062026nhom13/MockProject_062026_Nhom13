using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NursingHome.Application.Features.LocationInfrastructure.Queries;

namespace NursingHome.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public InventoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("equipment-supply-inventory")]
    public async Task<IActionResult> GetEquipmentSupplyInventory(
        [FromQuery] GetEquipmentSupplyInventoryQuery query,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(query, cancellationToken);

        return Ok(response);
    }
}
