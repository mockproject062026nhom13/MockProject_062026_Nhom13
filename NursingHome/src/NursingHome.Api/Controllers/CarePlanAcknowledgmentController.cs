using MediatR;
using Microsoft.AspNetCore.Mvc;
using NursingHome.Application.Features.CarePlanAcknowledgments.Queries;
using NursingHome.Domain.Constants;
using NursingHome.Infrastructure.Authorization;

namespace NursingHome.Api.Controllers;

[ApiController]
[Route("api/care-plans")]
[PermissionAuthorize(PermissionConstants.CarePlanView)]
public class CarePlanAcknowledgmentController : ControllerBase
{
    private readonly IMediator _mediator;

    public CarePlanAcknowledgmentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{carePlanId:long}/acknowledgment")]
    public async Task<IActionResult> GetCarePlanAcknowledgment(
        long carePlanId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetCarePlanAcknowledgmentQuery { CarePlanId = carePlanId },
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }
}
