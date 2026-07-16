using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NursingHome.Application.Abstractions.Authorization;
using NursingHome.Infrastructure.Persistence.DbContexts;

namespace NursingHome.Infrastructure.Authorization
{
  public sealed class PermissionService : IPermissionService
  {
    private readonly IMemoryCache _cache;
    private readonly NursingHomeDbContext _db;

    public PermissionService(IMemoryCache cache, NursingHomeDbContext db)
    {
      _cache = cache;
      _db = db;
    }

    public async Task<bool> HasPermissionAsync(long userId, string actionCode, CancellationToken ct = default)
    {
      var cacheKey = $"perm:{userId}";

      if (!_cache.TryGetValue(cacheKey, out HashSet<string>? permissions))
      {
        permissions = await LoadPermissionsAsync(userId, ct);

        if (permissions.Count == 0)
        {
          return false;
        }

        var options = new MemoryCacheEntryOptions
        {
          AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
          SlidingExpiration = TimeSpan.FromMinutes(3)
        };

        _cache.Set(cacheKey, permissions, options);
      }

      return permissions!.Contains(actionCode);
    }

    public Task InvalidateUserAsync(long userId)
    {
      _cache.Remove($"perm:{userId}");
      return Task.CompletedTask;
    }

    private async Task<HashSet<string>> LoadPermissionsAsync(long userId, CancellationToken cancellationToken)
    {
      var permissions = await _db.Users
        .Where(u => u.Id == userId)
        .SelectMany(u => u.Role.Permissions)
        .Select(p => p.ActionCode)
        .Distinct()
        .ToListAsync(cancellationToken);

      return permissions.ToHashSet(StringComparer.Ordinal);
    }
  }
}