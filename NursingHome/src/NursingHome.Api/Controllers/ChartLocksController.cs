using MediatR;
using Microsoft.AspNetCore.Mvc;
using NursingHome.Application.Features.RiskAuditLogs.Commands;
using NursingHome.Application.Features.RiskAuditLogs.DTOs;
using NursingHome.Application.Features.RiskAuditLogs.Queries;
using NursingHome.Domain.Constants;
using NursingHome.Infrastructure.Authorization;

namespace NursingHome.Api.Controllers;

[ApiController]
[Route("api/incidents")]
public class ChartLocksController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChartLocksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{incidentId:long}/chart-lock")]
    [PermissionAuthorize(PermissionConstants.IncidentDetailView)]
    public async Task<IActionResult> GetIncidentChartLock(
        long incidentId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetIncidentChartLockQuery { IncidentId = incidentId },
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("{incidentId:long}/chart-unlock")]
    [PermissionAuthorize(PermissionConstants.ChartLockUnlockOverride)]
    public async Task<IActionResult> UnlockResidentChart(
        long incidentId,
        [FromBody] UnlockResidentChartRequestDto request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new UnlockResidentChartCommand
            {
                IncidentId = incidentId,
                Reason = request.Reason
            },
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }
}
