using MediatR;
using Microsoft.AspNetCore.Mvc;
using NursingHome.Application.Common;
using NursingHome.Application.Features.RiskAuditLogs.Commands;
using NursingHome.Application.Features.RiskAuditLogs.DTOs;
using NursingHome.Application.Features.RiskAuditLogs.Queries;
using Microsoft.AspNetCore.Authorization;
using NursingHome.Domain.Constants;

namespace NursingHome.Api.Controllers;

[ApiController]
[Route("api/v1/incident-severities")]
[Authorize(Roles = RoleConstants.FacilityManager + "," + RoleConstants.SystemAdministrator)]
public class IncidentSeverityController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    public record UpdateIncidentSeverityRequest(string? Description, string? Example);

    //GET
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _mediator.Send(new GetIncidentSeveritiesQuery());

        var response = ApiResponse<List<IncidentSeverityDto>>.CreateSuccess(
            data : result,
            statusCode: StatusCodes.Status200OK,
            message: "Lấy danh sách thành công."
        );

        return Ok(response);
    }

    //PUT
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIncidentSeverityAsync(long id, [FromBody] UpdateIncidentSeverityRequest bodyRequest)
    {
        var command = new UpdateIncidentSeverityCommand(id,bodyRequest.Description,bodyRequest.Example);

        await _mediator.Send(command);

        var response = ApiResponse<object>.CreateSuccess(
            data:null,
            statusCode : StatusCodes.Status200OK,
            message : "Cập nhật cấp độ sự cố thành công."
        );
        return Ok(response);
    }

}