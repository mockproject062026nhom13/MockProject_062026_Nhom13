using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Abstractions;
using NursingHome.Infrastructure.Persistence.DbContexts;
using NursingHome.Infrastructure.Persistence.Mappers;
namespace NursingHome.Infrastructure.Repositories.UserSecurity;
public class ActivateAccountRepository : IActivateAccountRepository
{
    private readonly NursingHomeDbContext _context;

    public ActivateAccountRepository(NursingHomeDbContext context)
    {
        _context = context;
    }
    public async Task<Domain.Entities.User?> GetByEmailAsync(string email)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);

        return user?.ToDomain(); // same with using UserMapper.ToDomain(user) but more elegant
    }

    public async Task UpdateAsync(Domain.Entities.User user)
    {
        var entity = await _context.Users
            .FirstAsync(x => x.Id == user.Id);

        entity.ApplyFromDomain(user);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}