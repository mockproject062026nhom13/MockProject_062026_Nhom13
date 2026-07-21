using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NursingHome.Application.Common;
using NursingHome.Application.Features.RiskAuditLogs.Commands;
using NursingHome.Domain.Constants;
using NursingHome.Infrastructure.Authorization;

namespace NursingHome.Api.Controllers;

//SC_037_M7-US-01_report_incident
[PermissionAuthorize(PermissionConstants.IncidentCreate)]
[Route("api/[controller]")]
[ApiController]

public class IncidentsController(IMediator _mediator) : ControllerBase
{
    [PermissionAuthorize(PermissionConstants.IncidentCreate)]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<long>>> CreateIncident([FromBody] CreateIncidentCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }
}