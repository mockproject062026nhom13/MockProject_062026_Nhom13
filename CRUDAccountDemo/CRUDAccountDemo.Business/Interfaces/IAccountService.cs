namespace CRUDAccountDemo.Business.Interfaces;

using CRUDAccountDemo.Business.DTOs;

public interface IAccountService
{
    Task<PagedResult<AccountResponse>> GetAllAsync(int page, int pageSize);
    Task<AccountResponse> GetByIdAsync(Guid id);
    Task<AccountResponse> CreateAsync(CreateAccountRequest request);
    Task<AccountResponse> UpdateAsync(Guid id, UpdateAccountRequest request);
    Task DeleteAsync(Guid id);
    Task<AccountResponse> DepositAsync(Guid id, DepositRequest request);
    Task<AccountResponse> WithdrawAsync(Guid id, WithdrawRequest request);
}
