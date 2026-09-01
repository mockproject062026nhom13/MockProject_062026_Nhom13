using NursingHome.Infrastructure.Persistence.Generated;
using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Abstractions.Auth;
using NursingHome.Application.Features.Auth.DTOs; 
using NursingHome.Infrastructure.Persistence.DbContexts; 
namespace NursingHome.Infrastructure.Persistence.Repositories.Auth; 

public class UserRepository(NursingHomeDbContext context) : IUserRepository
{
    private readonly NursingHomeDbContext _context = context;

    public async Task<UserAuthDto?> GetUserAuthInfoByEmailAsync(string email)
    {
       
        var userEntity = await _context.Set<User>()
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);

        if (userEntity == null) return null;

       
        return new UserAuthDto(
            Id: userEntity.Id,
            Email: userEntity.Email,
            MfaEnabled: userEntity.MfaEnabled,
            EmployeeCode: userEntity.EmployeeCode
        );
    }

    public async Task UpdateLoginTimeAsync(long userId)
{
    var userEntity = await _context.Set<User>().FindAsync(userId);
    
    if (userEntity != null)
    {
        
        userEntity.UpdateLastLogin(DateTimeOffset.UtcNow);
        
        await _context.SaveChangesAsync();
    }
}
}