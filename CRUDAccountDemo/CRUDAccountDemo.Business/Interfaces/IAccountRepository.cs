namespace CRUDAccountDemo.Business.Interfaces;

using CRUDAccountDemo.Business.Entities;

public interface IAccountRepository
{
    Task<(IEnumerable<Account> Accounts, int TotalCount)> GetAllAsync(int page, int pageSize);
    Task<Account?> GetByIdAsync(Guid id);
    Task<Account?> GetByEmailAsync(string email);
    Task AddAsync(Account account);
    Task UpdateAsync(Account account);
    Task DeleteAsync(Account account);
}
