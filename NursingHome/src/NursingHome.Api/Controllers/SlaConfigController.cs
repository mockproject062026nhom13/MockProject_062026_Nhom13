using MediatR;
using Microsoft.AspNetCore.Mvc;
using NursingHome.Application.Common;
using NursingHome.Application.Features.RiskAuditLogs.Commands;
using NursingHome.Application.Features.RiskAuditLogs.DTOs;
using NursingHome.Application.Features.RiskAuditLogs.Queries;
using NursingHome.Domain.Constants;
using NursingHome.Infrastructure.Authorization;

namespace NursingHome.Api.Controllers;

// SC_011 / AD-09 — SLA Configuration
[ApiController]
[Route("api/v1/sla-configs")]
[PermissionAuthorize(PermissionConstants.AdSlaConfigManage)]
public class SlaConfigController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    public record UpdateSlaConfigRequest(int SlaWindowHrs, string? RegulatoryBody);

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSlaConfigsQuery(), cancellationToken);

        var response = ApiResponse<List<SlaConfigDto>>.CreateSuccess(
            data: result,
            statusCode: StatusCodes.Status200OK,
            message: "Lấy danh sách cấu hình SLA thành công."
        );

        return Ok(response);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateAsync(long id, [FromBody] UpdateSlaConfigRequest bodyRequest, CancellationToken cancellationToken)
    {
        var command = new UpdateSlaConfigCommand(id, bodyRequest.SlaWindowHrs, bodyRequest.RegulatoryBody);

        await _mediator.Send(command, cancellationToken);

        var response = ApiResponse<object>.CreateSuccess(
            data: null,
            statusCode: StatusCodes.Status200OK,
            message: "Cập nhật cấu hình SLA thành công."
        );

        return Ok(response);
    }
}
