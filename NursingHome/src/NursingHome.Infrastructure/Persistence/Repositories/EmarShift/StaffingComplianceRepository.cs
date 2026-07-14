using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Abstractions.Repositories;
using NursingHome.Application.Features.Facilities.DTOs.Facility;
using NursingHome.Domain.Exceptions;
using NursingHome.Infrastructure.Persistence.DbContexts;

namespace NursingHome.Infrastructure.Persistence.Repositories.EmarShift;

public class StaffingComplianceRepository : IStaffingComplianceRepository
{
    private readonly NursingHomeDbContext _dbContext;

    public StaffingComplianceRepository(NursingHomeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<StaffingComplianceDto> GetComplianceAsync(long facilityId, DateOnly date, CancellationToken cancellationToken = default)
    {
        var facilityExists = await _dbContext.Facilities.AnyAsync(f => f.Id == facilityId, cancellationToken);
        if (!facilityExists)
        {
            throw new NotFoundException("Facility", facilityId);
        }

        // 1. Get Config
        var config = await _dbContext.StaffingConfigs
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.FacilityId == facilityId, cancellationToken);
        
        var minRequired = config?.MinHrsPerResidentDay ?? 3.5m; // Mặc định 3.5 nếu chưa có config

        // 2. Count Active Residents
        var census = await _dbContext.Residents
            .AsNoTracking()
            .Where(r => r.Status == "Active" && 
                        r.Admissions.Any(a => a.FacilityId == facilityId && a.DischargeDate == null))
            .CountAsync(cancellationToken);

        // 3. Calculate Required Hours
        var requiredHours = census * minRequired;

        // 4. Calculate Scheduled Hours
        var assignments = await _dbContext.ShiftAssignments
            .Include(sa => sa.Shift)
            .AsNoTracking()
            .Where(sa => sa.Shift.FacilityId == facilityId && sa.WorkDate == date)
            .ToListAsync(cancellationToken);

        decimal scheduledHours = 0;
        foreach (var assignment in assignments)
        {
            var start = assignment.Shift.StartTime;
            var end = assignment.Shift.EndTime;
            var duration = end.ToTimeSpan() - start.ToTimeSpan();
            if (duration.TotalHours < 0) 
            {
                // Nếu ca đêm qua ngày hôm sau (vd 22:00 -> 06:00)
                duration = duration.Add(TimeSpan.FromHours(24));
            }
            scheduledHours += (decimal)duration.TotalHours;
        }

        // 5. Evaluate
        var isCompliant = scheduledHours >= requiredHours;
        var actualPerResident = census > 0 ? scheduledHours / census : 0;

        return new StaffingComplianceDto
        {
            Census = census,
            MinRequired = minRequired,
            RequiredHours = Math.Round(requiredHours, 2),
            ScheduledHours = Math.Round(scheduledHours, 2),
            ActualHoursPerResident = Math.Round(actualPerResident, 2),
            IsCompliant = isCompliant,
            Status = isCompliant ? "COMPLIANT" : "NON_COMPLIANT"
        };
    }
}
