namespace CRUDAccountDemo.Business.Services;

using CRUDAccountDemo.Business.Constants;
using CRUDAccountDemo.Business.DTOs;
using CRUDAccountDemo.Business.Entities;
using CRUDAccountDemo.Business.Exceptions;
using CRUDAccountDemo.Business.Interfaces;

public class AccountService(IAccountRepository accountRepository) : IAccountService
{
    /// <summary>
    /// Returns a paginated list of accounts.
    /// </summary>
    public async Task<PagedResult<AccountResponse>> GetAllAsync(int page, int pageSize)
    {
        if (page < AccountConstants.MinPage)
            throw new ValidationException("page", $"Page must be at least {AccountConstants.MinPage}.");

        if (pageSize < 1 || pageSize > AccountConstants.MaxPageSize)
            throw new ValidationException("pageSize", $"Page size must be between 1 and {AccountConstants.MaxPageSize}.");

        var (accounts, totalCount) = await accountRepository.GetAllAsync(page, pageSize);
        var items = accounts.Select(MapToResponse);
        return new PagedResult<AccountResponse>(items, page, pageSize, totalCount);
    }

    /// <summary>
    /// Returns a single account by ID.
    /// </summary>
    public async Task<AccountResponse> GetByIdAsync(Guid id)
    {
        var account = await GetAccountOrThrowAsync(id);
        return MapToResponse(account);
    }

    /// <summary>
    /// Creates a new account.
    /// </summary>
    public async Task<AccountResponse> CreateAsync(CreateAccountRequest request)
    {
        ValidateEmail(request.Email);

        if (request.InitialBalance < AccountConstants.MinInitialBalance)
            throw new ValidationException("initialBalance", "Initial balance cannot be negative.");

        var normalizedEmail = NormalizeEmail(request.Email);

        var existing = await accountRepository.GetByEmailAsync(normalizedEmail);
        if (existing is not null)
            throw new ValidationException("email", "Email already exists.");

        var account = new Account(request.FullName.Trim(), normalizedEmail, request.InitialBalance);

        await accountRepository.AddAsync(account);
        return MapToResponse(account);
    }

    /// <summary>
    /// Updates an existing account's full name and email.
    /// </summary>
    public async Task<AccountResponse> UpdateAsync(Guid id, UpdateAccountRequest request)
    {
        var account = await GetAccountOrThrowAsync(id);

        ValidateEmail(request.Email);

        var normalizedEmail = NormalizeEmail(request.Email);
        if (!string.Equals(account.Email, normalizedEmail, StringComparison.Ordinal))
        {
            var existing = await accountRepository.GetByEmailAsync(normalizedEmail);
            if (existing is not null)
                throw new ValidationException("email", "Email already exists.");
        }

        account.UpdateInfo(request.FullName.Trim(), normalizedEmail);

        await accountRepository.UpdateAsync(account);
        return MapToResponse(account);
    }

    /// <summary>
    /// Deletes an account by ID.
    /// </summary>
    public async Task DeleteAsync(Guid id)
    {
        var account = await GetAccountOrThrowAsync(id);
        await accountRepository.DeleteAsync(account);
    }

    /// <summary>
    /// Deposits an amount into an account.
    /// </summary>
    public async Task<AccountResponse> DepositAsync(Guid id, DepositRequest request)
    {
        var account = await GetAccountOrThrowAsync(id);
        account.Deposit(request.Amount);
        await accountRepository.UpdateAsync(account);
        return MapToResponse(account);
    }

    /// <summary>
    /// Withdraws an amount from an account.
    /// </summary>
    public async Task<AccountResponse> WithdrawAsync(Guid id, WithdrawRequest request)
    {
        var account = await GetAccountOrThrowAsync(id);
        account.Withdraw(request.Amount);
        await accountRepository.UpdateAsync(account);
        return MapToResponse(account);
    }

    private async Task<Account> GetAccountOrThrowAsync(Guid id)
    {
        var account = await accountRepository.GetByIdAsync(id);
        if (account is null)
            throw new NotFoundException($"Account with ID '{id}' was not found.");
        return account;
    }

    private static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ValidationException("email", "Email is required.");

        try
        {
            var addr = new System.Net.Mail.MailAddress(email.Trim());
            if (addr.Address != email.Trim())
                throw new ValidationException("email", "Email is not valid.");
        }
        catch (FormatException)
        {
            throw new ValidationException("email", "Email is not valid.");
        }
    }

    private static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();

    private static AccountResponse MapToResponse(Account account) => new(
        account.Id,
        account.FullName,
        account.Email,
        account.Balance,
        account.CreatedAt,
        account.UpdatedAt);
}
