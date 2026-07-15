using Microsoft.EntityFrameworkCore;
using NursingHome.Infrastructure.Persistence.Generated;
using NursingHome.Application.Abstractions.Auth;
using NursingHome.Application.Features.UserSecurity.DTOs.Auth;

namespace NursingHome.Infrastructure.Persistence.Repositories.Auth;

public partial class UserRepository
{

    public async Task<AuthUserDto?> GetAuthUserByIdentifierAsync(string identifier, CancellationToken cancellationToken = default)
    {
        var user = await _context.Set<User>()
            .Include(u => u.Role)
            .ThenInclude(r => r.Permissions)
            .FirstOrDefaultAsync(u => (u.Email == identifier || u.PhoneNumber == identifier) && !u.IsDeleted, cancellationToken);

        if (user == null) return null;

        return new AuthUserDto
        {
            Id = user.Id,
            EmployeeCode = user.EmployeeCode,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            Status = user.Status,
            RoleName = user.Role.RoleName,
            Permissions = user.Role.Permissions.Select(p => p.ActionCode).ToList()
        };
    }

    public async Task UpdateLastLoginAsync(long userId, CancellationToken cancellationToken = default)
    {
        var user = await _context.Set<User>().FindAsync([userId], cancellationToken);
        if (user != null)
        {
            // Cập nhật LastLoginAt không cần exposing entity
            _context.Entry(user).Property(u => u.LastLoginAt).CurrentValue = DateTimeOffset.Now;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
    public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken)
    {
        return !await _context.Users.AnyAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<string?> GetLastEmployeeCodeAsync(CancellationToken cancellationToken)
    {
        return await _context.Users
            .Where(u => u.EmployeeCode.StartsWith("NHMS-"))
            .OrderByDescending(u => u.EmployeeCode)
            .Select(u => u.EmployeeCode)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsRoleExistsAsync(long roleId, CancellationToken cancellationToken)
    {
        return await _context.Roles.AnyAsync(r => r.Id == roleId, cancellationToken);
    }

    public async Task<bool> IsFacilityExistsAsync(long facilityId, CancellationToken cancellationToken)
    {
        return await _context.Facilities.AnyAsync(f => f.Id == facilityId, cancellationToken);
    }

    public async Task<long> AddUserAsync(UserCreationDto dto, CancellationToken cancellationToken)
    {
        // 1. Khởi tạo Entity từ DTO
        var newUser = new User(
            employeeCode: dto.EmployeeCode,
            email: dto.Email,
            firstName: dto.FirstName,
            middleName: dto.MiddleName,
            lastName: dto.LastName,
            phoneNumber: dto.PhoneNumber,
            roleId: dto.RoleId
        // licenseNumber default = null
        );

        await _context.Users.AddAsync(newUser, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        if (dto.AssignedFacilityId.HasValue)
        {
            newUser.UserFacilities.Add(new UserFacility(newUser.Id, dto.AssignedFacilityId.Value));
            await _context.SaveChangesAsync(cancellationToken); // Lưu lần 2 cho bảng trung gian
        }
        return newUser.Id;
    }
}
