using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using NursingHome.Domain.Constants;

namespace NursingHome.Infrastructure.Authorization;

public sealed class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
  private static readonly HashSet<string> KnownPermissionPolicies =
      typeof(PermissionConstants)
          .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
          .Where(field => field.IsLiteral && !field.IsInitOnly && field.FieldType == typeof(string))
          .Select(field => (string)field.GetRawConstantValue()!)
          .ToHashSet(StringComparer.Ordinal);

  public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
      : base(options)
  {
  }

  public override Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
  {
    if (KnownPermissionPolicies.Contains(policyName))
    {
      var policy = new AuthorizationPolicyBuilder()
          .RequireAuthenticatedUser()
          .AddRequirements(new PermissionAuthorizationRequirement(policyName))
          .Build();

      return Task.FromResult<AuthorizationPolicy?>(policy);
    }

    return base.GetPolicyAsync(policyName);
  }
}