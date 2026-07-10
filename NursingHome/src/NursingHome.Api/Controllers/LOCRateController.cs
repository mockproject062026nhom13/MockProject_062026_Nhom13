using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Common;
using NursingHome.Infrastructure.Persistence.DbContexts;
namespace NursingHome.Api.Controllers;

/// <summary>
/// Controller for managing LOC rates.
/// get admin/loc-rates (time series + current rate, defaut get current time)
/// get admin/loc-rates/{id}
/// put admin/loc-rates/ (update all loc rates) ( limit edit by condition: only allow edit if the rate is not used in any contract)
/// 
/// </summary>
/// 
[ApiController]
[Route("api/auth")]
public class LOCRateController : ControllerBase
{
    private readonly NursingHomeDbContext _context;

    public LOCRateController(NursingHomeDbContext context)
    {
        _context = context;
    }
    // todo:check permission  + facility_id + user_id
    [HttpGet("loc-rates")]
    public async Task<IActionResult> GetLocRates(
        [FromQuery] GetLocRatesRequest request)
    {
        
        //var userId = GetCurrentUserId(); // or get userId from JWT token or request context
        //mock userId
        var userId = Guid.Parse("your-user-id-here"); 
        
        //mock service
        //var result = await _service.GetLocRatesAsync( 
        var result = await GetLocRatesAsync(
            userId,
            request.FromDate,
            request.ToDate);

        return Ok(result);
    }
    public async Task<List<LocRateResponse>> GetLocRatesAsync(
        Guid userId,
        DateOnly fromDate,
        DateOnly toDate)
    {
        return await _context.user_facilities
            .Where(uf => uf.user_id == userId)
            .Join(
                _context.care_level_rates,
                uf => uf.facility_id,
                rate => rate.facility_id,
                (uf, rate) => new { uf, rate })
            .Where(x =>
                x.rate.effective_from <= toDate &&
                (x.rate.effective_to == null ||
                x.rate.effective_to >= fromDate))
            .OrderBy(x => x.rate.facility_id)
            .ThenBy(x => x.rate.care_level_id)
            .ThenBy(x => x.rate.effective_from)
            .Select(x => new LocRateResponse
            {
                FacilityId = x.rate.facility_id,
                CareLevelId = x.rate.care_level_id,
                LocRate = x.rate.daily_rate,
                EffectiveFrom = x.rate.effective_from,
                EffectiveTo = x.rate.effective_to,
                IsPrimary = x.uf.is_primary
            })
            .ToListAsync();
    }
}

public class GetLocRatesRequest
{
    public DateOnly FromDate { get; set; }

    public DateOnly ToDate { get; set; }
}

public class LocRateResponse
{
    public Guid FacilityId { get; set; }

    public long CareLevelId { get; set; }

    public decimal LocRate { get; set; }

    public DateOnly EffectiveFrom { get; set; }

    public DateOnly? EffectiveTo { get; set; }

    public bool IsPrimary { get; set; }
}

