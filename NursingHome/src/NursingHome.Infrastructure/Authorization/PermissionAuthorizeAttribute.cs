using Microsoft.AspNetCore.Authorization;

namespace NursingHome.Infrastructure.Authorization;

public sealed class PermissionAuthorizeAttribute : AuthorizeAttribute
{
  public PermissionAuthorizeAttribute(string policy)
  {
    Policy = policy;
  }
}