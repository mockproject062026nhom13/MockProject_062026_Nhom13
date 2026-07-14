using NursingHome.Application.Features.UserSecurity.DTOs.Auth;

namespace NursingHome.Application.Abstractions.Repositories;

public interface IUserRepository
{
    Task<AuthUserDto?> GetAuthUserByIdentifierAsync(string identifier, CancellationToken cancellationToken = default);
    Task UpdateLastLoginAsync(long userId, CancellationToken cancellationToken = default);
}
