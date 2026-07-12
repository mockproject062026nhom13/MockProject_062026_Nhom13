namespace NursingHome.Application.Abstractions.Auth;

public interface IOtpService
{
    //verify OTP code
    Task<bool> ValidateOtpAsync(string email, string otpCode);
    //Generate a new code and send it
    Task<bool> GenerateAndSendOtpAsync (string email);
}