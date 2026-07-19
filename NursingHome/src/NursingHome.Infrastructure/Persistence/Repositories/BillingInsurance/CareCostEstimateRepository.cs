using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Abstractions.BillingInsurance;
using NursingHome.Application.Features.BillingInsurance.DTOs; 
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Infrastructure.Persistence.Generated;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NursingHome.Infrastructure.Persistence.Repositories.BillingInsurance;

public class CareCostEstimateRepository(NursingHomeDbContext dbContext) : ICareCostEstimateRepository
{
    public async Task<CareCostDataDto?> GetCareCostDataAsync(long carePlanId, CancellationToken cancellationToken = default)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        // Only use 1 query
        var result = await dbContext.Set<CarePlan>()
            .AsNoTracking()
            .Where(cp => cp.Id == carePlanId && !cp.IsDeleted)
            .Select(cp => new
            {
                CarePlan = cp,
                Resident = dbContext.Set<Resident>().FirstOrDefault(r => r.Id == cp.ResidentId),
                
                // Left join: Get room from bed
                Room = dbContext.Set<Bed>()
                    .Where(b => b.Id == dbContext.Set<Resident>().FirstOrDefault(r => r.Id == cp.ResidentId).BedId)
                    .Select(b => dbContext.Set<Room>().FirstOrDefault(rm => rm.Id == b.RoomId))
                    .FirstOrDefault(),

                // Get FacilityId mà resident chưa xuất viện (hồ sơ nhập viện chưa có ngày xuất viện)
                ActiveFacilityId = dbContext.Set<Admission>()
                    .Where(a => a.ResidentId == cp.ResidentId && a.DischargeDate == null)
                    .Select(a => a.FacilityId)
                    .FirstOrDefault(),

                // Get the newest active LocHistory
                ActiveCareLevelHistory = dbContext.Set<ResidentCareLevelHistory>()
                    .Where(h => h.ResidentId == cp.ResidentId && (h.EndDate == null || h.EndDate >= today))
                    .OrderByDescending(h => h.StartDate)
                    .FirstOrDefault()
            })
            
            .Select(x => new
            {
                x.CarePlan,
                x.Resident,
                x.Room,
                CareLevelName = dbContext.Set<CareLevel>()
                    .Where(cl => cl.Id == x.ActiveCareLevelHistory.CareLevelId)
                    .Select(cl => cl.LevelName)
                    .FirstOrDefault(),
                
                // Tra đúng bảng giá dựa trên cả Chi nhánh và Thời gian hiện tại
                ActiveRate = dbContext.Set<CareLevelRate>()
                    .Where(rt => rt.CareLevelId == x.ActiveCareLevelHistory.CareLevelId 
                              && rt.FacilityId == x.ActiveFacilityId 
                              && rt.EffectiveFrom <= today 
                              && (rt.EffectiveTo == null || rt.EffectiveTo >= today))
                    .OrderByDescending(rt => rt.EffectiveFrom)
                    .FirstOrDefault()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (result == null || result.Resident == null) return null;

        // Ánh xạ dữ liệu sang DTO 
        return new CareCostDataDto(
            CarePlanId: result.CarePlan.Id,
            CarePlanStatus: result.CarePlan.Status,
            ResidentFirstName: result.Resident.FirstName,
            ResidentMiddleName: result.Resident.MiddleName,
            ResidentLastName: result.Resident.LastName,
            RoomNumber: result.Room?.RoomNumber,
            RoomType: result.Room?.RoomType,
            CareLevelName: result.CareLevelName,
            CareLevelRateId: result.ActiveRate?.Id,
            LocDailyRate: result.ActiveRate?.DailyRate ?? 0m
        );
    }
}