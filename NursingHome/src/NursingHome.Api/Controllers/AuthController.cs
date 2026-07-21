using Microsoft.AspNetCore.Mvc;
using MediatR;
using NursingHome.Application.Features.Auth.Commands;
using NursingHome.Application.Common;
using NursingHome.Application.Features.UserSecurity.Commands.Login;
using NursingHome.Application.Features.Auth.DTOs;

namespace NursingHome.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    //private readonly ISender _sender;

    // public AuthController(ISender sender)
    // {
    //     _sender = sender;
    // }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(command, cancellationToken);
        return Ok(ApiResponse<LoginResponse>.CreateSuccess(response));
    }

    //1.OTP authentication API
    [HttpPost("mfa/verify")]
    public async Task<IActionResult> VerifyMfaAsync([FromBody] VerifyMfaCommand command)
    {
        var result = await _mediator.Send(command);

        var response = ApiResponse<AuthResultDto>.CreateSuccess(
            data : result,
            statusCode: StatusCodes.Status200OK,
            message : "Xác thực 2 bước thành công"

        );
        return Ok(response);
    }

    //2.Resend OTP code
    [HttpPost("mfa/resend")]
    public async Task<IActionResult> ResendMfaAsync([FromBody] ResendMfaCommand command)
    {
        await _mediator.Send(command);
        var response = ApiResponse<object>.CreateSuccess(
            data: null,
            statusCode : StatusCodes.Status200OK,
            message: "Mã xác thực mới đã được gửi đến email của bạn . Vui lòng kiểm tra hộp thư."
        );
        return Ok(response);
    }

    ///
    

}