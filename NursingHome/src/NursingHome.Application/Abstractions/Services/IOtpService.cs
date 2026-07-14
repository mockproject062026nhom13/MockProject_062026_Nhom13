namespace NursingHome.Application.Abstractions.Services;

public interface IOtpService
{
    Task<string> GenerateOtpAsync(long userId, CancellationToken cancellationToken = default);
    Task<bool> VerifyOtpAsync(long userId, string otp, CancellationToken cancellationToken = default);
}
