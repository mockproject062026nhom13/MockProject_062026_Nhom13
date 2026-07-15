using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NursingHome.Application.Abstractions.Auth;


namespace NursingHome.Infrastructure.Services; 
public class OtpService (IMemoryCache memoryCache,ILogger<OtpService> logger):IOtpService
{
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly ILogger<OtpService> _logger = logger;
    
    public async Task<bool> GenerateAndSendOtpAsync(string email)
    {
        string otpCode = Random.Shared.Next(100000,999999).ToString();
        string cacheKey = $"OTP_{email}";

        //load code into ram within 3 minutes
        _memoryCache.Set(cacheKey,otpCode,TimeSpan.FromMinutes(3));
        _logger.LogInformation($"[Mô phỏng gửi email] Mã OTP của {email} là : {otpCode}");

        await Task.CompletedTask;
        return true;
    }
    public async Task<bool> ValidateOtpAsync(string email, string otpCode)
    {
        string cacheKey = $"OTP_{email}";

        //TryGetValue returns true if found and populates the savedOtp variable with the data.
        if(_memoryCache.TryGetValue(cacheKey,out string? savedOtp))
        {
            if(savedOtp == otpCode)
            {
                _memoryCache.Remove(cacheKey);
                return true;
            }
        }

        await Task.CompletedTask;
        return false;

    }
}