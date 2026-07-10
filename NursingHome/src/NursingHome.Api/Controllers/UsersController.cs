using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NursingHome.Application.Features.UserSecurity.Commands;

namespace NursingHome.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
    {
        var response = await _mediator.Send(command);
        
        if (response.Success)
        {
            return Ok(response); 
        }

        return BadRequest(response); 
    }
}