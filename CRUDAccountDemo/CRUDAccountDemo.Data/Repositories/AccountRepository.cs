namespace CRUDAccountDemo.Data.Repositories;

using CRUDAccountDemo.Business.Entities;
using CRUDAccountDemo.Business.Interfaces;
using CRUDAccountDemo.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

public class AccountRepository(ApplicationDbContext context) : IAccountRepository
{
    /// <summary>
    /// Returns a paginated list of accounts ordered by creation date descending.
    /// </summary>
    public async Task<(IEnumerable<Account> Accounts, int TotalCount)> GetAllAsync(int page, int pageSize)
    {
        var totalCount = await context.Accounts.CountAsync();

        var accounts = await context.Accounts
            .OrderByDescending(a => a.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (accounts, totalCount);
    }

    public async Task<Account?> GetByIdAsync(Guid id)
        => await context.Accounts.FindAsync(id);

    public async Task<Account?> GetByEmailAsync(string email)
        => await context.Accounts.FirstOrDefaultAsync(a => a.Email == email);

    public async Task AddAsync(Account account)
    {
        await context.Accounts.AddAsync(account);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Account account)
    {
        context.Accounts.Update(account);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Account account)
    {
        context.Accounts.Remove(account);
        await context.SaveChangesAsync();
    }
}
