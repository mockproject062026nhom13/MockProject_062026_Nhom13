using NursingHome.Application.Models.Auth;

namespace NursingHome.Application.Abstractions.Authentication;

public interface IJwtTokenService
{
    string GenerateToken(AuthUserDto user);
}
