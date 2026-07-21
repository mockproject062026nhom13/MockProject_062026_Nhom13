using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Abstractions.UserSecurity;
using NursingHome.Application.Features.UserSecurity.DTOs;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Infrastructure.Persistence.Generated;

namespace NursingHome.Infrastructure.Persistence.Repositories.UserSecurity;

public class UserAdminRepository(NursingHomeDbContext context) : IUserAdminRepository
{
    private readonly NursingHomeDbContext _context = context;

    public async Task<(IReadOnlyList<UserListItemDto> Items, int TotalCount)> GetUsersAsync(
        string? search, long? roleId, string? status, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Set<User>().AsNoTracking().Include(u => u.Role).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(u => u.Email.Contains(s) || (u.PhoneNumber != null && u.PhoneNumber.Contains(s)));
        }

        if (roleId.HasValue)
            query = query.Where(u => u.RoleId == roleId.Value);

        if (!string.IsNullOrWhiteSpace(status))
        {
            var st = status.Trim().ToUpperInvariant();
            query = st switch
            {
                "ACTIVE" => query.Where(u => !u.IsDeleted && u.Status == "ACTIVE"),
                "INVITED" => query.Where(u => !u.IsDeleted && u.Status == "INVITED"),
                "SUSPENDED" or "LOCKED" => query.Where(u => !u.IsDeleted && u.Status == "LOCKED"),
                "DEACTIVATED" or "INACTIVE" => query.Where(u => u.IsDeleted || u.Status == "INACTIVE"),
                _ => query
            };
        }

        var total = await query.CountAsync(cancellationToken);

        var rows = await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new
            {
                u.Id,
                u.FirstName,
                u.MiddleName,
                u.LastName,
                u.Email,
                u.PhoneNumber,
                u.RoleId,
                RoleName = u.Role.RoleName,
                u.Status,
                u.MfaEnabled,
                u.LastLoginAt,
                u.IsDeleted
            })
            .ToListAsync(cancellationToken);

        var items = rows.Select(u => new UserListItemDto(
            Id: u.Id,
            Name: BuildFullName(u.FirstName, u.MiddleName, u.LastName),
            Email: u.Email,
            PhoneNumber: u.PhoneNumber,
            RoleId: u.RoleId,
            RoleName: u.RoleName,
            Status: u.Status,
            StatusDisplay: MapStatusDisplay(u.Status, u.IsDeleted),
            MfaEnabled: u.MfaEnabled,
            LastLoginAt: u.LastLoginAt
        )).ToList();

        return (items, total);
    }

    public async Task<UserStatisticsDto> GetStatisticsAsync(CancellationToken cancellationToken = default)
    {
        var rows = await _context.Set<User>().AsNoTracking()
            .Select(u => new { u.Status, u.IsDeleted })
            .ToListAsync(cancellationToken);

        var active = rows.Count(u => !u.IsDeleted && u.Status == "ACTIVE");
        var invited = rows.Count(u => !u.IsDeleted && u.Status == "INVITED");
        var suspendedOrDeactivated = rows.Count(u => u.IsDeleted || u.Status == "LOCKED" || u.Status == "INACTIVE");

        return new UserStatisticsDto(
            Total: rows.Count,
            Active: active,
            Invited: invited,
            SuspendedOrDeactivated: suspendedOrDeactivated);
    }

    public async Task<UserDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var u = await _context.Set<User>().AsNoTracking().Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (u is null) return null;

        return new UserDetailDto(
            Id: u.Id,
            EmployeeCode: u.EmployeeCode,
            FirstName: u.FirstName,
            MiddleName: u.MiddleName,
            LastName: u.LastName,
            FullName: BuildFullName(u.FirstName, u.MiddleName, u.LastName),
            Email: u.Email,
            PhoneNumber: u.PhoneNumber,
            RoleId: u.RoleId,
            RoleName: u.Role.RoleName,
            LicenseNumber: u.LicenseNumber,
            Status: u.Status,
            StatusDisplay: MapStatusDisplay(u.Status, u.IsDeleted),
            MfaEnabled: u.MfaEnabled,
            LastLoginAt: u.LastLoginAt,
            IsDeleted: u.IsDeleted,
            CreatedAt: u.CreatedAt);
    }

    public async Task<bool> UpdateProfileAsync(
        long id, string firstName, string? middleName, string lastName,
        string? phoneNumber, string? licenseNumber, long roleId, CancellationToken cancellationToken = default)
    {
        var user = await _context.Set<User>().FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        if (user is null) return false;

        user.UpdateProfile(firstName, middleName, lastName, phoneNumber, licenseNumber, roleId);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ChangeStatusAsync(long id, string action, CancellationToken cancellationToken = default)
    {
        var user = await _context.Set<User>().FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        if (user is null) return false;

        switch (action)
        {
            case "DEACTIVATE": user.Deactivate(); break;
            case "REACTIVATE": user.Reactivate(); break;
            case "SUSPEND": user.Suspend(); break;
            default: return false;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static string BuildFullName(string first, string? middle, string last)
        => string.Join(' ', new[] { first, middle, last }.Where(s => !string.IsNullOrWhiteSpace(s)));

    // Bảng users chỉ có INVITED/ACTIVE/INACTIVE/LOCKED — ánh xạ sang nhãn hiển thị của màn hình.
    private static string MapStatusDisplay(string status, bool isDeleted)
    {
        if (isDeleted) return "Deactivated";

        return status?.ToUpperInvariant() switch
        {
            "INVITED" => "Invited",
            "ACTIVE" => "Active",
            "LOCKED" => "Suspended",
            "INACTIVE" => "Deactivated",
            _ => status ?? "Unknown"
        };
    }
}
