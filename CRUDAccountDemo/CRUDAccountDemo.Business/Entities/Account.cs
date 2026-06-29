namespace CRUDAccountDemo.Business.Entities;

using CRUDAccountDemo.Business.Exceptions;

public class Account
{
    private const decimal MinAmount = 0m;

    // For EF Core materialization
    private Account() { }

    public Account(string fullName, string email, decimal initialBalance)
    {
        Id = Guid.NewGuid();
        FullName = fullName;
        Email = email;
        Balance = initialBalance;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public decimal Balance { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Updates the account's full name and email.
    /// </summary>
    public void UpdateInfo(string fullName, string email)
    {
        FullName = fullName;
        Email = email;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Deposits an amount into the account. Amount must be greater than zero.
    /// </summary>
    public void Deposit(decimal amount)
    {
        if (amount <= MinAmount)
            throw new ValidationException("amount", "Deposit amount must be greater than zero.");

        Balance += amount;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Withdraws an amount from the account. Amount must be greater than zero and balance must be sufficient.
    /// </summary>
    public void Withdraw(decimal amount)
    {
        if (amount <= MinAmount)
            throw new ValidationException("amount", "Withdrawal amount must be greater than zero.");

        if (Balance < amount)
            throw new BusinessException("Insufficient balance.");

        Balance -= amount;
        UpdatedAt = DateTime.UtcNow;
    }
}
