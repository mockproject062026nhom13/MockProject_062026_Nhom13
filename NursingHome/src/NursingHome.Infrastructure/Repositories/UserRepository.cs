using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Abstractions.Repositories;
using NursingHome.Application.Models.Auth;
using NursingHome.Infrastructure.Persistence.DbContexts;

namespace NursingHome.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly NursingHomeDbContext _dbContext;

    public UserRepository(NursingHomeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AuthUserDto?> GetAuthUserByEmployeeCodeAsync(string employeeCode, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users
            .Include(u => u.Role)
            .ThenInclude(r => r.Permissions)
            .FirstOrDefaultAsync(u => u.EmployeeCode == employeeCode && !u.IsDeleted, cancellationToken);

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
        var user = await _dbContext.Users.FindAsync(new object[] { userId }, cancellationToken);
        if (user != null)
        {
            // Cập nhật LastLoginAt không cần exposing entity
            _dbContext.Entry(user).Property(u => u.LastLoginAt).CurrentValue = DateTimeOffset.Now;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
