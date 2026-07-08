using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Features.UserSecurity.Commands;
using NursingHome.Infrastructure.Persistence.Generated;

using NursingHome.Infrastructure.Persistence.DbContexts;

namespace NursingHome.Infrastructure.Repositories.UserSecurity;

public class UserRepository : IUserRepository
{
    private readonly NursingHomeDbContext _dbContext;

    public UserRepository(NursingHomeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken)
    {
        return !await _dbContext.users.AnyAsync(u => u.email == email, cancellationToken);
    }

    public async Task<string?> GetLastEmployeeCodeAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.users
            .Where(u => u.employee_code.StartsWith("NHMS-"))
            .OrderByDescending(u => u.employee_code)
            .Select(u => u.employee_code)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Guid> AddUserAsync(UserCreationDto dto, CancellationToken cancellationToken)
    {
        // 1. Khởi tạo Entity từ DTO
        var newUser = new user(
            employeeCode: dto.EmployeeCode,
            email: dto.Email,
            firstName: dto.FirstName,
            middleName: dto.MiddleName,
            lastName: dto.LastName,
            phoneNumber: dto.PhoneNumber,
            roleId: dto.RoleId
            // licenseNumber default = null
        );

        if (dto.AssignedFacilityId.HasValue)
        {
            newUser.user_facilities.Add(new user_facility(newUser.id, dto.AssignedFacilityId.Value));
        }

        await _dbContext.users.AddAsync(newUser, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return newUser.id;
    }
}