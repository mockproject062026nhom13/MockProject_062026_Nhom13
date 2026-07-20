using MediatR;
using Microsoft.AspNetCore.Mvc;
using NursingHome.Domain.Constants;
using NursingHome.Infrastructure.Authorization;
using NursingHome.Application.Features.RiskAuditLogs.Queries;


namespace NursingHome.Api.Controllers;

[ApiController]
[Route("api/v1/incidents")]
[PermissionAuthorize(PermissionConstants.IncidentListView)]
public class IncidentController: ControllerBase
{
    private readonly IMediator _mediator;
    public IncidentController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet]
    public async Task<IActionResult> GetIncidents(
        [FromQuery] string status = "All",
        [FromQuery] string severity = "All",
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var query  = new GetIncidentListQuery(status, severity,page,pageSize);

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(
        [FromQuery] int? month,
        [FromQuery] int? year)
    {
        //If Frontend does not transmit month/year, the default is to take the current system time
        var targetMonth = month ?? DateTimeOffset.UtcNow.Month;
        var targetYear = year ?? DateTimeOffset.UtcNow.Year;

        //Packaging Request
        var query = new GetIncidentSummaryQuery(targetMonth,targetYear);

        var result = await _mediator.Send(query);

        return Ok(result);
    }
}