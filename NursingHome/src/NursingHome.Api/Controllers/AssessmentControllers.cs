using MediatR;
using Microsoft.AspNetCore.Mvc;
using NursingHome.Application.Features.CareLevelResidents.Commands;

namespace NursingHome.Api.Controllers;

[ApiController]
[Route("api/assessments")]
public class AssessmentController : ControllerBase
{
    private readonly IMediator _mediator;

    public AssessmentController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost]
    public async Task<IActionResult> CreateAssessment(
        CreateAssessmentCommand command)
    {
        var response = await _mediator.Send(command);

        if (response.Success)
            return Ok(response);

        return BadRequest(response);
    }
}
/*
//assessedBy: role nurse
{
  "assessment": {
    "residentId": 1,
    "assessedBy": 5, 
    "assessmentDetails": [
      {
        "metricId": 1,
        "score": 18,
        "notes": "Independent"
      },
      {
        "metricId": 2,
        "score": 14,
        "notes": "Requires assistance"
      },
      {
        "metricId": 3,
        "score": 20,
        "notes": "Independent"
      },
      {
        "metricId": 4,
        "score": 8,
        "notes": "Extensive assistance"
      },
      {
        "metricId": 5,
        "score": 4,
        "notes": "Total dependence"
      }
    ],
    "vitalSigns": {
      "bloodPressureSystolic": 125,
      "bloodPressureDiastolic": 80,
      "heartRateBpm": 72,
      "respiratoryRate": 16,
      "temperatureFahrenheit": 98.6,
      "spo2Percentage": 97,
      "painScale": 3,
      "notes": "Vital signs stable during assessment."
    }
  }
}
*/