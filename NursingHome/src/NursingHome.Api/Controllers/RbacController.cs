using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NursingHome.Application.Features.Rbac.Queries;

namespace NursingHome.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RbacController : ControllerBase
{
    private readonly IMediator _mediator;

    public RbacController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("role-permission-matrix")]
    public async Task<IActionResult> GetRolePermissionMatrix()
    {
        var response = await _mediator.Send(new GetRolePermissionMatrixQuery());

        return Ok(response);
    }
}
