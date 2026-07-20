using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using NursingHome.Application.Features.BillingInsurance.Queries;
using Microsoft.AspNetCore.Authorization;
using NursingHome.Infrastructure.Authorization;
using NursingHome.Domain.Constants;

namespace NursingHome.Api.Controllers;

//SC_035_M2-US-09_cost-billing-panel
[Authorize]
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