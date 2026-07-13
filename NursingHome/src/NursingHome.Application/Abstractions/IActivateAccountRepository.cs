using System.Threading;
using System.Threading.Tasks;
// not using infastructure because we want to keep the application layer independent of the infrastructure layer. The application layer should only depend on abstractions, not concrete implementations.
using NursingHome.Domain.Entities;

namespace NursingHome.Application.Abstractions;
public class ActivateAccountRequest
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string ConfirmPassword { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }
}

public class ActivateAccountResponse
{
    public string Email { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;
}

public interface IActivateAccountRepository

{   
    
    Task<Domain.Entities.User?> GetByEmailAsync(string email);
    // already handled in domain layer, so we don't need to check here again
    // Task<bool> ActivateAsync(
    //     long userId,
    //     string passwordHash,
    //     string? phoneNumber);
    Task UpdateAsync(Domain.Entities.User user);
    Task SaveChangesAsync();
}