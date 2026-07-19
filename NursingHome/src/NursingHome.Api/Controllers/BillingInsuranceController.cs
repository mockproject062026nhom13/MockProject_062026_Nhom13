using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using NursingHome.Application.Features.BillingInsurance.Queries;

namespace NursingHome.Api.Controllers;

[ApiController]
[Route("api/v1/billing-insurance")]
public class BillingInsuranceController(IMediator mediator) : ControllerBase
{
    [HttpGet("care-plans/{id}/estimate")]
    public async Task<IActionResult> GetCareCostEstimate(long id)
    {
        var query = new GetCareCostEstimateQuery(id);
        var result = await mediator.Send(query);

        return Ok(result);
    }
}