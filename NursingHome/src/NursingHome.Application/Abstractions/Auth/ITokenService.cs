namespace NursingHome.Application.Abstractions.Auth;

public interface ITokenService
{
    
    string GenerateAccessToken(Guid userId, string email);
    
    
    string GenerateRefreshToken();
}