using Microsoft.EntityFrameworkCore;
using NursingHome.Infrastructure.Persistence.Generated;
using NursingHome.Application.Abstractions.Auth;

namespace NursingHome.Infrastructure.Persistence.Repositories.Auth;

public partial class UserRepository
{

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