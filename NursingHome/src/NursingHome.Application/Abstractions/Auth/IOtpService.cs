namespace NursingHome.Application.Abstractions.Auth;

public interface IOtpService
{
    Task<bool> ValidateOtpAsync(string email, string otpCode);
}