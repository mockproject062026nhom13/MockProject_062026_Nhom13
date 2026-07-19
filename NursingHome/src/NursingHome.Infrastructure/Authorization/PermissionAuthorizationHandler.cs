using Microsoft.AspNetCore.Authorization;
using NursingHome.Application.Abstractions;
using NursingHome.Application.Abstractions.Authorization;
using NursingHome.Domain.Constants;

namespace NursingHome.Infrastructure.Authorization
{
  public sealed class PermissionAuthorizationHandler
      : AuthorizationHandler<PermissionAuthorizationRequirement>
  {
    private readonly IPermissionService _permissionService;
    private readonly ICurrentUserService _currentUser;

    public PermissionAuthorizationHandler(
        IPermissionService permissionService,
        ICurrentUserService currentUser)
    {
      _permissionService = permissionService;
      _currentUser = currentUser;
    }

    protected override async Task HandleRequirementAsync(
       AuthorizationHandlerContext context,
       PermissionAuthorizationRequirement requirement)
    {
      if (context.User?.Identity?.IsAuthenticated != true)
      {
        return;
      }

      if (context.User.IsInRole(RoleConstants.SystemAdministrator))
      {
        context.Succeed(requirement);
        return;
      }

      if (_currentUser.UserId is not long userId)
      {
        return;
      }

      if (await _permissionService.HasPermissionAsync(userId, requirement.ActionCode))
      {
        context.Succeed(requirement);
      }
    }
  }
}