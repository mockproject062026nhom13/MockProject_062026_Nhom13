using MediatR;
using Microsoft.AspNetCore.Mvc;
using NursingHome.Application.Common;
using NursingHome.Application.Features.CareLevelResidents.Queries; 
using NursingHome.Application.Features.CareLevelResidents.DTOs;

namespace NursingHome.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CareLevelResidentsController(IMediator _mediator) : ControllerBase
{
    //SC_017_Select resident list
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ResidentListDto>>>> GetResidentList([FromQuery] GetResidentListQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    //SC_017_Statistics resident by status
    [HttpGet("statistics")]
    public async Task<ActionResult<ApiResponse<ResidentStatisticsDto>>> GetStatistics([FromQuery] GetResidentStatisticsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}