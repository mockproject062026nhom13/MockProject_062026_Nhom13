using System;

namespace NursingHome.Infrastructure.Persistence.Generated;

/// <summary>
/// Activate the user account with the provided password hash and optional phone number.
/// </summary>
public partial class User
{
    public void ActivateAccount(string passwordHash, string? phoneNumber)
    {

        // maybe need to clean logic here, but for now, we just check if the account is already activated or locked.
        if (IsDeleted)
            throw new InvalidOperationException("User has been deleted.");

        if (Status == "LOCKED")
            throw new InvalidOperationException("Account is locked.");

        if (Status == "Enabled")
            throw new InvalidOperationException("Account has already been activated.");

        PasswordHash = passwordHash;
        PhoneNumber = phoneNumber;
        Status = "Enabled";
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    public void ApplyFromDomain(Domain.Entities.User domain)
    {
        PasswordHash = domain.PasswordHash;
        PhoneNumber = domain.PhoneNumber;
        Status = domain.Status.ToString();
        UpdatedAt = domain.UpdatedAt;
        IsDeleted = domain.IsDeleted;
        DeletedAt = domain.DeletedAt;
    }
}