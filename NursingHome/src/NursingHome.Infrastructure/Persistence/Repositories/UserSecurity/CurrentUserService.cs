using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using NursingHome.Application.Abstractions;

namespace NursingHome.Infrastructure.Persistence.Repositories.UserSecurity;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public long? UserId
    {
        get
        {
            var value = _httpContextAccessor.HttpContext?
                .User
                .FindFirstValue(ClaimTypes.NameIdentifier);

            return long.TryParse(value, out var userId)
                ? userId
                : null;
        }
    }

    public string? IpAddress =>
        _httpContextAccessor.HttpContext?
            .Connection
            .RemoteIpAddress?
            .ToString();
}
