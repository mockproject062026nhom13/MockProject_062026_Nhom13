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
        var query = context.CarePlans
            .Where(cp=>!cp.IsDeleted && cp.AssignedNurseId == currentUserId);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            string search = searchTerm.Trim();
            query = query.Where(cp => cp.Recident.FirsName.Contains(search) ||
            cp.Recident.LastName.Contains(search) ||
            (cp.Recident.Bed != null && cp.Recident.Bed.Room.RoomNumber.Contains(search))
            );
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(cp => cp.Status == status);

        }

        if(!string.IsNullOrWhiteSpace(reviewStatus) && reviewStatus.Equals("OVERDUE", StringComparison.OrdinalIgnoreCase))
        {
            query = query.Where(cp=>cp.NextReviewDate < DateTimeOffset.UtcNow);
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
            ResidentName = cp.Resident.FirsName + " " + cp.Resident.LastName,
            RoomNumber = cp.Resident.Bed != null ? cp.Resident.Bed.Room.RoomNumber: "N/A",
            LocTier = cp.Resident.LocTier,
            Status = cp.Status,
            LastReviewDate = cp.UpdatedAt,
            NextReviewDate = cp.NextReviewDate,
            AssignedNurseName = cp.AssignedNurse != null ? cp.AssignedNurse.FirsName + " " + cp.AssignedNurse.LastName: string.Empty
        }).ToListAsync(cancellationToken);

        return new PageResult<CarePlanDto>(items,totalCount,pageIndex,pageSize);



    }

    // Statistic
    public async Task<CarePlanStatisticDto>GetStatisticsByNurseIdAysnc(long nurseId, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;
        var stats = await context.CarePlans
        .Where(cp=> cp.AssignedNurseId == nurseId)
        .GroupBy(cp => 1)
        .Select(group => new CarePlanStatisticDto
        {
            TotalPlans = group.Count(),
            DraftCount = group.Count(cp => cp.Status == "DRAFT"),
            PendingReviewCount = group.Count(cp => cp.Status == "PENDING_REVIEW"),
            ReviewDueCount = group.Count(cp => 
                cp.NextReviewDate <= now &&
                cp.Status != "CANCELLED" &&
                cp.Status != "COMPLETED")

        }).FistOrDefaultAsync(cancellationToken);
        return stats ?? new CarePlanStatisticDto();
    }
}