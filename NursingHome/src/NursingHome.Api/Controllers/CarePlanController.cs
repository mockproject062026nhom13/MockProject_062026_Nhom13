using MediatR;
using Microsoft.AspNetCore.Mvc;
using NursingHome.Application.Features.CareLevelResidents.Queries;
using NursingHome.Application.Features.CareLevelResidents.DTOs;
using NursingHome.Application.Features.CareLevelResidents.Commands;
using NursingHome.Domain.Constants;
using NursingHome.Infrastructure.Authorization;
namespace NursingHome.Api.Controllers;

/// <summary>
/// Controller for managing care plans.
/// </summary>
// todo: validate + permission
[ApiController]
[Route("api/auth")]
public class CarePlanController : ControllerBase
{
    private readonly IMediator _mediator;

    public CarePlanController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get care plan information by care plan id.
    /// </summary>
    [HttpGet("care-plans/status")]
    [PermissionAuthorize(PermissionConstants.CarePlanView)]
    public async Task<IActionResult> GetCarePlanIdsByStatusAsync(
        [FromQuery] string status)
    {
        var query = new GetCarePlanIdsByStatusQuery(status);

        var result = await _mediator.Send(query);

        return StatusCode(result.StatusCode, result);
    }
    [HttpGet("care-plans/{carePlanId:long}")]
    [PermissionAuthorize(PermissionConstants.CarePlanView)]
    public async Task<IActionResult> GetCarePlanAsync(
        [FromRoute] long carePlanId)
    {
        var query = new GetCarePlanQuery(carePlanId);

        var result = await _mediator.Send(query);

        return StatusCode(result.StatusCode, result);
    }

    //check permission
    [HttpPut("care-plans/{carePlanId:long}/status")]
    [PermissionAuthorize(PermissionConstants.CarePlanCreateEdit)]
    public async Task<IActionResult> UpdateCarePlanStatusCommand(
        [FromRoute] long carePlanId,
        [FromBody] UpdateCarePlanStatusRequest request)
    {
        var command = new UpdateCarePlanStatusCommand{
            CarePlanId = carePlanId,
            Status = request.Status};

        var result = await _mediator.Send(command);

        return StatusCode(result.StatusCode, result);   
    }
}