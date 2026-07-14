using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NursingHome.Application.Features.AdmissionEhrCarePlan.Queries;

namespace NursingHome.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdmissionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdmissionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("pre-admission-screenings/{screeningId:long}")]
    public async Task<IActionResult> GetPreAdmissionScreeningDetail(
        long screeningId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetPreAdmissionScreeningDetailQuery { ScreeningId = screeningId },
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }
}
