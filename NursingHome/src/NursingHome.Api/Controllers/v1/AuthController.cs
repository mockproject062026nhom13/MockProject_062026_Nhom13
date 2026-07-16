using MediatR;
using Microsoft.AspNetCore.Mvc;
using NursingHome.Application.Common;
using NursingHome.Application.Features.UserSecurity.Commands.Login;

namespace NursingHome.Api.Controllers.v1;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(command, cancellationToken);
        return Ok(ApiResponse<LoginResponse>.CreateSuccess(response));
    }
}
