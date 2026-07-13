using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Abstractions;
using NursingHome.Infrastructure.Persistence.DbContexts;
namespace NursingHome.Infrastructure.Persistence.Mappers;
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
    // already handled in domain layer, so we don't need to check here again
    // public async Task<bool> ActivateAsync(long userId, string passwordHash, string? phoneNumber)
    // {
    //     var user = await _context.Users
    //         .FirstOrDefaultAsync(u => u.Id == userId);

    //     if (user == null)
    //         return false;

    //     user.ActivateAccount(passwordHash, phoneNumber);
    //     await SaveChangesAsync();
    //     return true;
    // }
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