using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Features.RiskAuditLogs.DTOs;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Infrastructure.Persistence.Generated;

namespace NursingHome.Infrastructure.Persistence.Repositories;

public class IncidentRepository : IIncidentRepository
{
    private readonly NursingHomeDbContext _context;
    public IncidentRepository(NursingHomeDbContext context)
    {
        _context = context;
    }
    public async Task<List<IncidentListItemDto>> GetPagedIncidentsAsync(
        string statusFilter,
        string severityFilter,
        int skip,
        int take,
        CancellationToken cancellationToken)
    {
        var query = _context.Set<Incident>()
            .AsNoTracking()
            .AsQueryable();

        if(!string.Equals(statusFilter,"All",StringComparison.OrdinalIgnoreCase))
            query = query.Where(i=>i.Status == statusFilter);

        if(!string.Equals(severityFilter,"All",StringComparison.OrdinalIgnoreCase))
            query = query.Where(i=>i.Severity.LevelName == severityFilter);
        
        var currentTime = DateTimeOffset.UtcNow;

        var resultList = await query
            .OrderByDescending(i=>i.ReportedAt)
            .Skip(skip)
            .Take(take)
            .Select(i=> new IncidentListItemDto(
                i.Id,
                i.Resident.FirstName + " " + i.Resident.LastName + " • " + 
                (i.Resident.Bed != null && i.Resident.Bed.Room != null ? i.Resident.Bed.Room.RoomNumber : "") + 
                (i.Resident.Bed != null ? i.Resident.Bed.BedNumber : ""),
                i.IncidentType,
                i.Severity.LevelName,
                i.ReportedAt.ToString(),
                i.Status == "CLOSED" ? "—" : (i.SlaDeadline < currentTime ? "OVERDUE" : "In time"),
                i.Status,
                i.Resident.IsChartLocked?"Locked" : "Unlocked"
            )).ToListAsync(cancellationToken);
        
        return resultList;
    }

    public async Task<IncidentSummaryDto> GetIncidentSummaryAsync(int month,int year,CancellationToken cancellationToken)
    {
        var currentTime = DateTimeOffset.UtcNow;

        var monthData = await _context.Set<Incident>()
            .AsNoTracking()
            .Where(i=>i.ReportedAt.Month == month && i.ReportedAt.Year == year)
            .Select(i=> new{
                Status = i.Status,
                SlaDeadline = i.SlaDeadline,
                IsChartLocked = i.Resident.IsChartLocked

            }).ToListAsync(cancellationToken);

        int total = monthData.Count;
        int openCount = monthData.Count(i=> i.Status == "OPEN" || i.Status == "UNDER_INVESTIGATION");
        int overdueCount = monthData.Count(i=>i.Status != "CLOSED" && i.SlaDeadline < currentTime);
        int chartLockedCount = monthData.Count(i=> i.IsChartLocked);
        int resolvedCount = monthData.Count(i => i.Status == "CLOSED");

        return new IncidentSummaryDto(
            TotalThisMonth: total,
            Open : openCount,
            Overdue : overdueCount,
            ChartLocked : chartLockedCount,
            Resolved : resolvedCount
        );
    }
}