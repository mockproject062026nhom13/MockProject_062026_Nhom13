using NursingHome.Application.Features.UserSecurity.DTOs.Auth;

namespace NursingHome.Application.Abstractions.Authentication;

public interface IJwtTokenService
{
    string GenerateToken(AuthUserDto user);
    string GeneratePreAuthToken(AuthUserDto user);
}
