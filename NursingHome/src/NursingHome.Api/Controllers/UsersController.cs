using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NursingHome.Application.Features.UserSecurity.Commands;
using NursingHome.Application.Features.UserSecurity.Queries;
using NursingHome.Domain.Constants;
using NursingHome.Infrastructure.Authorization;

namespace NursingHome.Api.Controllers;

// SC_004 / AD-01 — User List & quản trị tài khoản (SC_005/AD-02 = create/edit).
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public record UpdateUserRequest(string FullName, string? PhoneNumber, long RoleId, string? LicenseNumber);
    public record ChangeUserStatusRequest(string Action);

    // Danh sách + search (email/phone) + filter (role/status), sort theo ngày tạo mới nhất.
    [HttpGet]
    [PermissionAuthorize(PermissionConstants.AdUserListView)]
    public async Task<IActionResult> GetUsers([FromQuery] GetUsersQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    // 4 thẻ đếm Total/Active/Invited/Suspended+Deactivated.
    [HttpGet("statistics")]
    [PermissionAuthorize(PermissionConstants.AdUserListView)]
    public async Task<IActionResult> GetStatistics(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUserStatisticsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [PermissionAuthorize(PermissionConstants.AdUserListView)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [PermissionAuthorize(PermissionConstants.AdUserCreateEdit)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{id:long}")]
    [PermissionAuthorize(PermissionConstants.AdUserCreateEdit)]
    public async Task<IActionResult> UpdateUser(long id, [FromBody] UpdateUserRequest body, CancellationToken cancellationToken)
    {
        var command = new UpdateUserCommand(id, body.FullName, body.PhoneNumber, body.RoleId, body.LicenseNumber);
        var response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }

    // Deactivate / Reactivate / Suspend — soft-delete (AD-04, giữ lịch sử).
    [HttpPatch("{id:long}/status")]
    [PermissionAuthorize(PermissionConstants.AdUserActivateToggle)]
    public async Task<IActionResult> ChangeStatus(long id, [FromBody] ChangeUserStatusRequest body, CancellationToken cancellationToken)
    {
        var command = new ChangeUserStatusCommand(id, body.Action);
        var response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }
}
