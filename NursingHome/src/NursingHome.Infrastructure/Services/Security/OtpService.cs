using Microsoft.Extensions.Caching.Memory;
using NursingHome.Application.Abstractions.Services;

namespace NursingHome.Infrastructure.Services.Security;

public class OtpService : IOtpService
{
    private readonly IMemoryCache _cache;
    private static readonly TimeSpan OtpExpiration = TimeSpan.FromMinutes(5);

    public OtpService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public Task<string> GenerateOtpAsync(long userId, CancellationToken cancellationToken = default)
    {
        // Sinh mã OTP ngẫu nhiên 6 số
        var random = new Random();
        string otp = random.Next(100000, 999999).ToString();
        
        string cacheKey = $"otp_{userId}";

        // Lưu vào MemoryCache
        _cache.Set(cacheKey, otp, OtpExpiration);

        return Task.FromResult(otp);
    }

    public Task<bool> VerifyOtpAsync(long userId, string otp, CancellationToken cancellationToken = default)
    {
        string cacheKey = $"otp_{userId}";

        if (_cache.TryGetValue(cacheKey, out string? cachedOtp))
        {
            if (cachedOtp == otp)
            {
                // Xóa OTP sau khi verify thành công để tránh dùng lại (Replay attack)
                _cache.Remove(cacheKey);
                return Task.FromResult(true);
            }
        }

        return Task.FromResult(false);
    }
}
