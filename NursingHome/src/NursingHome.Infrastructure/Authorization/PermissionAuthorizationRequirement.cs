using Microsoft.AspNetCore.Authorization;

namespace NursingHome.Infrastructure.Authorization;

public sealed class PermissionAuthorizationRequirement : IAuthorizationRequirement
{
  public PermissionAuthorizationRequirement(string actionCode)
  {
    ArgumentException.ThrowIfNullOrWhiteSpace(actionCode);
    ActionCode = actionCode;
  }

  public string ActionCode { get; }
}