using Microsoft.AspNetCore.Mvc;
using MediatR;
using NursingHome.Application.Features.Auth.Commands;

namespace NursingHome.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("mfa/verify")]
    public async Task<IActionResult> VerifyMfaAsync([FromBody] VerifyMfaCommand command)
    {
        var result = await _mediator.Send(command);

        // Trả về đúng format API Document
        return Ok(new
        {
            success = true,
            statusCode = 200,
            message = "2-Step verification successful. Login completed.",
            data = result,
            errors = (object?)null,
            pagination = (object?)null
        });
    }
}