using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Common;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Infrastructure.Persistence.Generated;

using NursingHome.Api.LOCRateDTOs;

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
    // todo:check permission  + facility_id + user_id + seperate architecture
    // todo: audit log for post and put
    // todo: standardization for response and error handling

    // todo: add http post to add new loc rate if nessary
    [HttpPost("admin/loc-rates")]
    public async Task<IActionResult> CreateLocRateAsync(
        [FromBody] CreateLocRateRequest request)
    {
        // mock userId
        // todo: get it from jwt token in the future
        var userId = 1L;

        // TODO: Validate request
        // - EffectiveFrom <= EffectiveTo
        // - DailyRate > 0
        // - CareLevelId hợp lệ
        // - FacilityId hợp lệ

        // TODO: Check user có quyền với Facility này

        // TODO: Check overlap
        // Nếu không cho phép khoảng thời gian bị chồng nhau thì:
        //
        // var overlap = await _context.CareLevelRates.AnyAsync(x =>
        //     x.FacilityId == request.FacilityId &&
        //     x.CareLevelId == request.CareLevelId &&
        //     x.EffectiveFrom <= (request.EffectiveTo ?? DateOnly.MaxValue) &&
        //     (x.EffectiveTo ?? DateOnly.MaxValue) >= request.EffectiveFrom);
        //
        // if (overlap)
        //     return Conflict("Date range overlaps existing rate.");

        var entity = CareLevelRate.Create(
            request.CareLevelId,
            request.FacilityId,
            request.DailyRate,
            request.EffectiveFrom,
            request.EffectiveTo);
        // todo: add audit log for create loc rate
        _context.CareLevelRates.Add(entity);

        await _context.SaveChangesAsync();

        // return CreatedAtAction(
        //     nameof(GetLocRatesAsync),
        //     new
        //     {
        //         fromDate = entity.EffectiveFrom,
        //         toDate = entity.EffectiveTo
        //     },
        //     new
        //     {
        //         entity.FacilityId,
        //         entity.CareLevelId,
        //         entity.DailyRate,
        //         entity.EffectiveFrom,
        //         entity.EffectiveTo
        //     });
        return Ok(new
        {
            entity.FacilityId,
            entity.CareLevelId,
            entity.DailyRate,
            entity.EffectiveFrom,
            entity.EffectiveTo
        });
    }

    [HttpGet("admin/loc-rates")]
    public async Task<IActionResult> GetLocRatesAsync(
        [FromQuery] GetLocRatesRequest request)
    {
        // mock userId
        // todo: get it from jwt token in the future
        var userId = 1L;

        var result = await _context.UserFacilities
            .Where(uf => uf.UserId == userId)
            .Join(
                _context.CareLevelRates,
                uf => uf.FacilityId,
                rate => rate.FacilityId,
                (uf, rate) => new { uf, rate })
            .Where(x =>
                x.rate.EffectiveFrom <= request.ToDate &&
                (x.rate.EffectiveTo ==  null ||
                x.rate.EffectiveTo >= request.FromDate))
            .OrderBy(x => x.rate.FacilityId)
            .ThenBy(x => x.rate.CareLevelId)
            .ThenBy(x => x.rate.EffectiveFrom)
            .Select(x => new LocRateResponse
            {
                FacilityId = x.rate.FacilityId,
                CareLevelId = x.rate.CareLevelId,
                LocRate = x.rate.DailyRate,
                EffectiveFrom = x.rate.EffectiveFrom,
                EffectiveTo = x.rate.EffectiveTo,
                IsPrimary = x.uf.IsPrimary
            })
            .ToListAsync();

        return Ok(result);
    }
    // UserFacility permission validation will be handled by middleware.
    // Validation of EffectiveFrom and EffectiveTo will be handled by domain logic or middleware.
    [HttpPut("admin/loc-rates")]
    public async Task<IActionResult> UpdateLocRateAsync(
        [FromBody] UpdateLocRateRequest request)
    {
        // mock userId
        // TODO: Get userId from JWT token
        var userId = 1L;

        // TODO:
        // Do not validate facility access here.

        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var entity = await _context.CareLevelRates
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (entity == null)
            {
                return NotFound();
            }

            // TODO:
            // Get old LOC rate by calling existing GET loc-rates logic/API.
            // This will be implemented later.
            decimal? oldRate = entity.DailyRate;

            // TODO:
            // Validate EffectiveFrom / EffectiveTo rule.
            // Allow overlap temporarily.
            // Business rule will be implemented later.

            entity.Update(
                request.CareLevelId,
                request.FacilityId,
                request.DailyRate,
                request.EffectiveFrom,
                request.EffectiveTo);

            // TODO:
            // After updating (or creating new version in the future),
            // create audit log row.
            //
            // Action:
            // EDIT_LOC_RATE
            //
            // JSON format:
            //
            // {
            //    "facilityId":1,
            //    "careLevelId":2,
            //    "oldRate":100,
            //    "newRate":120
            // }

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return Ok(new
            {
                entity.Id,
                entity.FacilityId,
                entity.CareLevelId,
                entity.DailyRate,
                entity.EffectiveFrom,
                entity.EffectiveTo
            });
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
// todo: apply ApiResponse and ApiError to the response of this controller
public class GetLocRatesRequest
{
    public DateOnly FromDate { get; set; }

    public DateOnly ToDate { get; set; }
}

public class LocRateResponse
{
    public long FacilityId { get; set; }

    public long CareLevelId { get; set; }

    public decimal LocRate { get; set; }

    public DateOnly EffectiveFrom { get; set; }

    public DateOnly? EffectiveTo { get; set; }

    public bool IsPrimary { get; set; }
}

