using MediatR;
using Microsoft.AspNetCore.Mvc;
using NursingHome.Application.Features.LocationInfrastructure.Commands.CreateLOCRate;
using NursingHome.Application.Features.LocationInfrastructure.Commands.UpdateLOCRate;
using NursingHome.Application.Features.LocationInfrastructure.Queries.GetLOCRates;

namespace NursingHome.Api.Controllers;

/// <summary>
/// Controller for managing LOC rates.
/// </summary>
[ApiController]
[Route("api/auth")]
public class LOCRateController : ControllerBase
{
    private readonly IMediator _mediator;

    public LOCRateController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create a new LOC rate.
    /// </summary>
    [HttpPost("admin/loc-rates")]
    public async Task<IActionResult> CreateLocRateAsync(
        [FromBody] CreateLOCRateCommand command)
    {
        var result = await _mediator.Send(command);

        return StatusCode(result.StatusCode, result);
    }

    /*
    /// <summary>
    /// Get LOC rates.
    /// </summary>
    /// todo: had commented in interface, implement it if nessary
    /// already do: case only id, maybe need for only date start
    /// 
    /// */
    [HttpGet("admin/loc-rates")]
    public async Task<IActionResult> GetLocRatesAsync(
        [FromQuery] GetLOCRatesQuery query)
    {
        var result = await _mediator.Send(query);

        return StatusCode(result.StatusCode, result);
    }
    

    /// <summary>
    /// Update an existing LOC rate.
    /// </summary>
    [HttpPut("admin/loc-rates")]
    public async Task<IActionResult> UpdateLocRateAsync(
        [FromBody] UpdateLOCRateCommand command)
    {
        var result = await _mediator.Send(command);

        return StatusCode(result.StatusCode, result);
    }
}