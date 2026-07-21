using MediatR;
using Microsoft.AspNetCore.Mvc;
using NursingHome.Application.Features.AdmissionEhrCarePlan.Queries;
using NursingHome.Domain.Constants;
using NursingHome.Infrastructure.Authorization;

namespace NursingHome.Api.Controllers;

[ApiController]
[Route("api/assessments")]
public class LocClassificationController : ControllerBase
{
    private readonly IMediator _mediator;

    public LocClassificationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{assessmentId:long}/loc-classification")]
    public async Task<IActionResult> GetLocClassificationResult(
        long assessmentId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetLocClassificationResultQuery { AssessmentId = assessmentId },
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("~/api/residents/{residentId:long}/loc-history")]
    [PermissionAuthorize(PermissionConstants.LocHistoryView)]
    public async Task<IActionResult> GetLocHistory(
        long residentId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetLocHistoryQuery { ResidentId = residentId },
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }
}
