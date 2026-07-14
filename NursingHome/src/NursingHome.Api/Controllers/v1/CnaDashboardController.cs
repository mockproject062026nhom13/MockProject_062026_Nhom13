using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;
using NursingHome.Application.Common;
using NursingHome.Application.Features.CnaDashboard.DTOs;
using NursingHome.Application.Features.CnaDashboard.Queries;
using NursingHome.Application.Features.CnaDashboard.Commands;

namespace NursingHome.Api.Controllers.v1;

[ApiController]
[Authorize(Roles = "CNA_(Caregiver)")]
public class CnaDashboardController : ControllerBase
{
    private readonly ISender _sender;

    public CnaDashboardController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("api/cna/dashboard")]
    public async Task<IActionResult> GetDashboard(CancellationToken cancellationToken)
    {
        var subClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value 
                    ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(subClaim) || !long.TryParse(subClaim, out long cnaUserId))
        {
            return Unauthorized(ApiResponse<object>.CreateError(401, "Unauthorized or invalid CNA user identifier."));
        }

        var query = new GetCnaDashboardQuery(cnaUserId);
        var response = await _sender.Send(query, cancellationToken);
        return Ok(ApiResponse<CnaDashboardDto>.CreateSuccess(response));
    }

    [HttpPost("api/cna/tasks/{taskId}/complete")]
    public async Task<IActionResult> CompleteTask(long taskId, CancellationToken cancellationToken)
    {
        var subClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value 
                    ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(subClaim) || !long.TryParse(subClaim, out long cnaUserId))
        {
            return Unauthorized(ApiResponse<object>.CreateError(401, "Unauthorized or invalid CNA user identifier."));
        }

        var command = new CompleteCareTaskCommand(taskId, cnaUserId);
        await _sender.Send(command, cancellationToken);
        return Ok(ApiResponse<string>.CreateSuccess("Care task completed successfully."));
    }
}
