using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Abstractions.Repositories;
using NursingHome.Application.Features.Facilities.DTOs.Facility;
using NursingHome.Domain.Exceptions;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Infrastructure.Persistence.Generated;

namespace NursingHome.Infrastructure.Persistence.Repositories.EmarShift;

public class StaffingConfigRepository : IStaffingConfigRepository
{
    private readonly NursingHomeDbContext _dbContext;

    public StaffingConfigRepository(NursingHomeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<StaffingConfigDto?> GetByFacilityIdAsync(long facilityId, CancellationToken cancellationToken = default)
    {
        var config = await _dbContext.StaffingConfigs
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.FacilityId == facilityId, cancellationToken);

        if (config == null) return null;

        return new StaffingConfigDto
        {
            Id = config.Id,
            FacilityId = config.FacilityId,
            MinHrsPerResidentDay = config.MinHrsPerResidentDay,
            WarnBelowPercentage = config.WarnBelowPercentage
        };
    }

    public async Task<StaffingConfigDto> AddOrUpdateAsync(long facilityId, decimal minHrs, int warnPct, CancellationToken cancellationToken = default)
    {
        var facilityExists = await _dbContext.Facilities.AnyAsync(f => f.Id == facilityId, cancellationToken);
        if (!facilityExists)
        {
            throw new NotFoundException("Facility", facilityId);
        }

        var config = await _dbContext.StaffingConfigs
            .FirstOrDefaultAsync(c => c.FacilityId == facilityId, cancellationToken);

        if (config == null)
        {
            config = new StaffingConfig(facilityId, minHrs, warnPct);
            _dbContext.StaffingConfigs.Add(config);
        }
        else
        {
            config.UpdateConfig(minHrs, warnPct);
            _dbContext.StaffingConfigs.Update(config);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new StaffingConfigDto
        {
            Id = config.Id,
            FacilityId = config.FacilityId,
            MinHrsPerResidentDay = config.MinHrsPerResidentDay,
            WarnBelowPercentage = config.WarnBelowPercentage
        };
    }

    public async Task<StaffingConfigDto?> GetConfigWithRealBreakdownAsync(long facilityId, DateOnly date, CancellationToken cancellationToken = default)
    {
        var config = await _dbContext.StaffingConfigs
            .Include(c => c.Facility)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.FacilityId == facilityId, cancellationToken);

        if (config == null) return null;

        var census = await _dbContext.Residents
            .AsNoTracking()
            .Where(r => r.Status == "Active" && 
                        r.Admissions.Any(a => a.FacilityId == facilityId && a.DischargeDate == null))
            .CountAsync(cancellationToken);

        var assignments = await _dbContext.ShiftAssignments
            .Include(sa => sa.Shift)
            .Include(sa => sa.User).ThenInclude(u => u.Role)
            .AsNoTracking()
            .Where(sa => sa.Shift.FacilityId == facilityId && sa.WorkDate == date)
            .ToListAsync(cancellationToken);

        decimal dayCna = 0, dayNurse = 0;
        decimal eveningCna = 0, eveningNurse = 0;
        decimal nightCna = 0, nightNurse = 0;

        foreach (var assignment in assignments)
        {
            var start = assignment.Shift.StartTime;
            var end = assignment.Shift.EndTime;
            var duration = end.ToTimeSpan() - start.ToTimeSpan();
            if (duration.TotalHours < 0) duration = duration.Add(TimeSpan.FromHours(24));

            var hours = (decimal)duration.TotalHours;
            var shiftName = assignment.Shift.ShiftName.ToUpper();
            var roleName = assignment.User.Role.RoleName.ToUpper();
            var isNurse = roleName.Contains("NURSE") || roleName.Contains("LPN") || roleName.Contains("RN");
            var isCna = roleName.Contains("CNA") || roleName.Contains("CAREGIVER");

            if (shiftName.Contains("DAY") || shiftName.Contains("MORNING"))
            {
                if (isNurse) dayNurse += hours;
                if (isCna) dayCna += hours;
            }
            else if (shiftName.Contains("EVENING"))
            {
                if (isNurse) eveningNurse += hours;
                if (isCna) eveningCna += hours;
            }
            else if (shiftName.Contains("NIGHT"))
            {
                if (isNurse) nightNurse += hours;
                if (isCna) nightCna += hours;
            }
        }

        if (census > 0)
        {
            dayCna = Math.Round(dayCna / census, 2);
            dayNurse = Math.Round(dayNurse / census, 2);
            eveningCna = Math.Round(eveningCna / census, 2);
            eveningNurse = Math.Round(eveningNurse / census, 2);
            nightCna = Math.Round(nightCna / census, 2);
            nightNurse = Math.Round(nightNurse / census, 2);
        }
        else
        {
            dayCna = dayNurse = eveningCna = eveningNurse = nightCna = nightNurse = 0;
        }

        return new StaffingConfigDto
        {
            Id = config.Id,
            FacilityId = config.FacilityId,
            MinHrsPerResidentDay = config.MinHrsPerResidentDay,
            WarnBelowPercentage = config.WarnBelowPercentage,
            DayCnaHours = dayCna,
            DayNurseHours = dayNurse,
            EveningCnaHours = eveningCna,
            EveningNurseHours = eveningNurse,
            NightCnaHours = nightCna,
            NightNurseHours = nightNurse
        };
    }
}
