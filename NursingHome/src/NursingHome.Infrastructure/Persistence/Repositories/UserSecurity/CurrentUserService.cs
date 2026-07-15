using NursingHome.Application.Abstractions;
using NursingHome.Infrastructure.Persistence.DbContexts;

namespace NursingHome.Infrastructure.Persistence.Repositories.UserSecurity;

public sealed class CurrentUserService : ICurrentUserService
{
    // Mock user id
    // In a real application, you would retrieve the user id from the authentication context or token claims.
    public long? UserId => 1;

    // Mock IP address
    public string? IpAddress => "127.0.0.1";
}
