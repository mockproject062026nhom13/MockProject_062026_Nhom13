using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Abstractions.AdmissionEhrCarePlan;
using NursingHome.Application.Common.Models;
using NursingHome.Application.Features.AdmissionEhrCarePlan.DTOs;
using NursingHome.Infrastructure.Persistence.DbContexts;



namespace NursingHome.Infrastructure.Persistence.Repositories.AdmissionEhrCarePlan;

public class CarePlanRepository(NursingHomeDbContext context): ICarePlanRepository
{
    public async Task<PageResult<CarePlanDto>> GetCarePlanListAsync(
        string? searchTerm ,
        string? status,
        string? reviewStatus,
        long currentUserId,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken)
    {
        //apply filters
        // var query = context.CarePlans
        //     .Where(cp=>!cp.IsDeleted && cp.AssignedNurseId == currentUserId);
        //  TẠM TẮT LỌC THEO AssignedNurseId
        var query = context.CarePlans.Where(cp => !cp.IsDeleted);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            string search = searchTerm.Trim();
            query = query.Where(cp => cp.Resident.FirstName.Contains(search) ||
            cp.Resident.LastName.Contains(search) ||
            (cp.Resident.Bed != null && cp.Resident.Bed.Room.RoomNumber.Contains(search))
            );
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(cp => cp.Status == status);

        }

        
        // if(!string.IsNullOrWhiteSpace(reviewStatus) && reviewStatus.Equals("OVERDUE", StringComparison.OrdinalIgnoreCase))
        // {
        //     query = query.Where(cp=>cp.NextReviewDate < DateTimeOffset.UtcNow);
        // }

        // 2. HARD CODE LOGIC LỌC OVERDUE (Vì chưa có NextReviewDate)
        if(!string.IsNullOrWhiteSpace(reviewStatus) && reviewStatus.Equals("OVERDUE", StringComparison.OrdinalIgnoreCase))
    {
        // Lấy những bản ghi có Id chẵn coi như là Overdue (Để test Frontend)
        query = query.Where(cp => cp.Id % 2 == 0); 
    }

        //Count the total to paginate
        int totalCount = await query.CountAsync(cancellationToken);

        //projection and pagination
        var items = await query 
        .OrderByDescending(cp=>cp.UpdatedAt)
        .Skip((pageIndex - 1)*pageSize)
        .Take(pageSize)
        .Select(cp=>new CarePlanDto
        {
            Id = cp.Id,
            ResidentName = cp.Resident.FirstName + " " + cp.Resident.LastName,
            RoomNumber = cp.Resident.Bed != null ? cp.Resident.Bed.Room.RoomNumber: "N/A",
            //LocTier = cp.Resident.LocTier,
            LocTier = "Tier 2",
            Status = cp.Status,
            // LastReviewDate = cp.UpdatedAt,
            // NextReviewDate = cp.NextReviewDate,
            // AssignedNurseName = cp.AssignedNurse != null ? cp.AssignedNurse.FirsName + " " + cp.AssignedNurse.LastName: string.Empty

            // HARD CODE MOCK DATA CHO CÁC CỘT THIẾU:
            LastReviewDate = DateTimeOffset.UtcNow.AddDays(-30), 
            NextReviewDate = DateTimeOffset.UtcNow.AddDays(7),   
            AssignedNurseName = "Nguyễn Văn Mock"
        }).ToListAsync(cancellationToken);

        return new PageResult<CarePlanDto>(items,totalCount,pageIndex,pageSize);



    }

    // Statistic
    public async Task<CarePlanStatisticDto>GetStatisticsByNurseIdAsync(long nurseId, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;
        var stats = await context.CarePlans
        //.Where(cp=> cp.AssignedNurseId == nurseId)
        .GroupBy(cp => 1)
        .Select(group => new CarePlanStatisticDto
        {
            TotalPlans = group.Count(),
            DraftCount = group.Count(cp => cp.Status == "DRAFT"),
            PendingReviewCount = group.Count(cp => cp.Status == "PENDING_REVIEW"),
            // ReviewDueCount = group.Count(cp => 
            //     cp.NextReviewDate <= now &&
            //     cp.Status != "CANCELLED" &&
            //     cp.Status != "COMPLETED")

            ReviewDueCount = group.Count(cp => cp.Status == "PENDING_REVIEW")

        }).FirstOrDefaultAsync(cancellationToken);
        return stats ?? new CarePlanStatisticDto();
    }
}