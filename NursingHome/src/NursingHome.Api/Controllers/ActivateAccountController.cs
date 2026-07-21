

using MediatR;
using Microsoft.AspNetCore.Mvc;
using NursingHome.Application.Features.UserSecurity.Commands;
using NursingHome.Domain.Constants;
using NursingHome.Infrastructure.Authorization;
namespace NursingHome.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class ActivateAccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public ActivateAccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("activate-account")]
    public async Task<IActionResult> ActivateAccount(
        ActivateAccountCommand command)
    {
        var response = await _mediator.Send(command);

        if (response.Success)
            return Ok(response);

        return BadRequest(response);
    }
}