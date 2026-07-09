using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Abstractions.Auth;
using NursingHome.Application.Features.Auth.DTOs;
using NursingHome.Infrastructure.Persistence.Generated; 
using NursingHome.Infrastructure.Persistence.DbContexts; 
namespace NursingHome.Infrastructure.Persistence.Repositories.Auth; 

public class UserRepository(NursingHomeDbContext context) : IUserRepository
{
    private readonly NursingHomeDbContext _context = context;

    public async Task<UserAuthDto?> GetUserAuthInfoByEmailAsync(string email)
    {
       
        var userEntity = await _context.users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.email == email);

        if (userEntity == null) return null;

       
        return new UserAuthDto(
            Id: userEntity.id,
            Email: userEntity.email,
            MfaEnabled: userEntity.mfa_enabled,
            EmployeeCode: userEntity.employee_code
        );
    }

    public async Task UpdateLoginTimeAsync(Guid userId)
    {
        var userEntity = await _context.users.FindAsync(userId);
        if (userEntity != null)
        {
            
            _context.Entry(userEntity).Property(u => u.last_login_at).CurrentValue = DateTimeOffset.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}