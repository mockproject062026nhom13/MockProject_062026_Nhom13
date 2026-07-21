using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NursingHome.Application.Features.CarePlans.Commands;
using NursingHome.Application.Features.CarePlans.DTOs;
using NursingHome.Domain.Constants;
using NursingHome.Infrastructure.Authorization;

namespace NursingHome.Api.Controllers;

// SC_027 / M2-US-02 — Care Plan Create (Draft → Submit for Review → Activate).
[ApiController]
[Route("api/care-plans")]
public class CarePlansController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    public record UpdateCarePlanRequest(List<CareAreaInput> CareAreas);

    [HttpPost]
    [PermissionAuthorize(PermissionConstants.CarePlanCreateEdit)]
    public async Task<IActionResult> Create([FromBody] CreateCarePlanCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:long}")]
    [PermissionAuthorize(PermissionConstants.CarePlanCreateEdit)]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateCarePlanRequest body, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateCarePlanCommand(id, body.CareAreas), cancellationToken);
        return Ok(result);
    }

    // "Submit for Review" — gửi DON duyệt.
    [HttpPost("{id:long}/submit")]
    [PermissionAuthorize(PermissionConstants.CarePlanCreateEdit)]
    public async Task<IActionResult> Submit(long id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new SubmitCarePlanCommand(id), cancellationToken);
        return Ok(result);
    }

    // Kích hoạt — sinh care_tasks từ interventions.
    [HttpPost("{id:long}/activate")]
    [PermissionAuthorize(PermissionConstants.LocConfirmOverride)]
    public async Task<IActionResult> Activate(long id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ActivateCarePlanCommand(id), cancellationToken);
        return Ok(result);
    }
}
