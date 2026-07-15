using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NursingHome.Application.Abstractions.Auth;


namespace NursingHome.Infrastructure.Services;

public class OtpService(IMemoryCache memoryCache, ILogger<OtpService> logger) : IOtpService
{
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly ILogger<OtpService> _logger = logger;
    private static readonly TimeSpan OtpExpiration = TimeSpan.FromMinutes(5);

    public async Task<bool> GenerateAndSendOtpAsync(string email)
    {
        string otpCode = Random.Shared.Next(100000, 999999).ToString();
        string cacheKey = $"OTP_{email}";

        //load code into ram within 3 minutes
        _memoryCache.Set(cacheKey, otpCode, TimeSpan.FromMinutes(3));
        _logger.LogInformation($"[Mô phỏng gửi email] Mã OTP của {email} là : {otpCode}");

        await Task.CompletedTask;
        return true;
    }
    public async Task<bool> ValidateOtpAsync(string email, string otpCode)
    {
        string cacheKey = $"OTP_{email}";

        //TryGetValue returns true if found and populates the savedOtp variable with the data.
        if (_memoryCache.TryGetValue(cacheKey, out string? savedOtp))
        {
            if (savedOtp == otpCode)
            {
                _memoryCache.Remove(cacheKey);
                return true;
            }
        }

        await Task.CompletedTask;
        return false;

    }

    public Task<string> GenerateOtpAsync(long userId, CancellationToken cancellationToken = default)
    {
        // Sinh mã OTP ngẫu nhiên 6 số
        var random = new Random();
        string otp = random.Next(100000, 999999).ToString();

        string cacheKey = $"otp_{userId}";

        // Lưu vào MemoryCache
        _memoryCache.Set(cacheKey, otp, OtpExpiration);

        return Task.FromResult(otp);
    }

    public Task<bool> VerifyOtpAsync(long userId, string otp, CancellationToken cancellationToken = default)
    {
        string cacheKey = $"otp_{userId}";

        if (_memoryCache.TryGetValue(cacheKey, out string? cachedOtp))
        {
            if (cachedOtp == otp)
            {
                // Xóa OTP sau khi verify thành công để tránh dùng lại (Replay attack)
                _memoryCache.Remove(cacheKey);
                return Task.FromResult(true);
            }
        }

        return Task.FromResult(false);
    }
}