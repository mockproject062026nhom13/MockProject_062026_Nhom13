namespace NursingHome.Application.Abstractions.Authorization
{
  public interface IPermissionService
  {
    Task<bool> HasPermissionAsync(long userId, string actionCode, CancellationToken ct = default);
    Task InvalidateUserAsync(long userId);
  }
}