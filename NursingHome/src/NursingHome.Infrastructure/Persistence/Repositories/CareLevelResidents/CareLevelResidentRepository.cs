using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Features.CareLevelResidents.Queries;
using NursingHome.Infrastructure.Persistence.DbContexts;

namespace NursingHome.Infrastructure.Persistence.Repositories.CareLevelResidents;

public class CareLevelResidentRepository(NursingHomeDbContext _context) : ICareLevelResidentRepository
{
    public async Task<(IReadOnlyList<ResidentListDto> Items, int TotalCount)> GetResidentListAsync(
        string? searchTerm, 
        string? status, 
        string? payerSource,
        int page, 
        int pageSize, 
        CancellationToken cancellationToken)
    {
        var query = _context.Residents
            .AsNoTracking()
            .Where(r => !r.IsDeleted);

        // filter by status
        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(r => r.Status == status.ToUpper());
        }

        // Search (by First Name hoặc Last Name)
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var search = searchTerm.Trim().ToLower();
            query = query.Where(r => 
                r.FirstName.ToLower().Contains(search) || 
                r.LastName.ToLower().Contains(search));
        }

        // filter by payerSource
        if (!string.IsNullOrWhiteSpace(payerSource))
        {
            query = query.Where(r => r.Invoices
                .SelectMany(i => i.Payments)
                .Any(p => p.PayerType == payerSource.ToUpper()));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var today = DateTime.UtcNow;

        var items = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new ResidentListDto(
                r.Id,
                string.IsNullOrWhiteSpace(r.MiddleName) 
                    ? r.FirstName + " " + r.LastName 
                    : r.FirstName + " " + r.MiddleName + " " + r.LastName,
                r.DateOfBirth.ToString("MM/dd/yyyy") + " (" + (today.Year - r.DateOfBirth.Year).ToString() + ")",
                r.Status,
                r.Bed != null && r.Bed.Room != null 
                    ? r.Bed.Room.RoomNumber + "-" + r.Bed.BedNumber 
                    : "Unassigned",
                r.Invoices
                    .OrderByDescending(i => i.CreatedAt)
                    .SelectMany(i => i.Payments)
                    .OrderByDescending(p => p.PaidAt)
                    .Select(p => p.PayerType)
                    .FirstOrDefault() ?? "N/A"
            ))
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<ResidentStatisticsDto> GetResidentStatisticsAsync(CancellationToken cancellationToken)
    {
        var statistics = await _context.Residents
            .AsNoTracking()
            .Where(r => !r.IsDeleted)
            .GroupBy(r => 1)
            .Select(g => new ResidentStatisticsDto(
                g.Count(), //count all resident (total)
                g.Count(r => r.Status == "ACTIVE"),
                g.Count(r => r.Status == "DISCHARGED"),
                g.Count(r => r.Status == "PENDING"),
                g.Count(r => r.Status == "DECEASED")
            ))
            .FirstOrDefaultAsync(cancellationToken);

        // return 0 if resident is empty
        return statistics ?? new ResidentStatisticsDto(0, 0, 0, 0, 0);
    }
}