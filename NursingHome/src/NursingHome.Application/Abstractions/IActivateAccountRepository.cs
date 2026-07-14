
namespace NursingHome.Application.Abstractions;


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