using MediatR;
using Microsoft.AspNetCore.Mvc;
using NursingHome.Application.Features.AdmissionEhrCarePlan.Queries;
using NursingHome.Domain.Constants;
using NursingHome.Infrastructure.Authorization;

namespace NursingHome.Api.Controllers;

[ApiController]
[Route("api/v1/care-plans")]
[PermissionAuthorize(PermissionConstants.CarePlanView)]
public class CarePlanController(IMediator mediator): ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCarePlanAsync ([FromQuery] GetCarePlansQuery query)
    {
        var result =  await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("statistics")]
    public async Task<IActionResult> GetStatistics()
    {
        var result = await mediator.Send(new GetCarePlanStatisticsQuery());
        return Ok(result);
    }
}
