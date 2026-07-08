namespace NursingHome.Infrastructure.Persistence.Generated;
// <summary>
/// Activate the user account with the provided password hash and optional phone number.
public partial class user
{
    public void ActivateAccount(string passwordHash, string? phoneNumber)
    {
        if (is_deleted)
            throw new InvalidOperationException("User has been deleted.");

        if (status == "LOCKED")
            throw new InvalidOperationException("Account is locked.");

        if (status == "Enabled")
            throw new InvalidOperationException("Account has already been activated.");

        password_hash = passwordHash;
        phone_number = phoneNumber;
        status = "Enabled";
        updated_at = DateTimeOffset.UtcNow;
    }
}