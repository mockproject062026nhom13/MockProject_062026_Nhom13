namespace NursingHome.Application.Abstractions.Auth;

public interface ITokenService
{
    string GenerateAccessToken(long userId, string email); 
    string GenerateRefreshToken();
}