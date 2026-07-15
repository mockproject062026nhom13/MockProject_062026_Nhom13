using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NursingHome.Application.Common;
using NursingHome.Application.Features.RiskAuditLogs.Commands;

namespace NursingHome.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IncidentsController(IMediator _mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ApiResponse<long>>> CreateIncident([FromBody] CreateIncidentCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }
}