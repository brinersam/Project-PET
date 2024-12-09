using Microsoft.AspNetCore.Authorization;

namespace ProjectPet.Framework.Authorization;

public class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        return Task.FromResult<AuthorizationPolicy?>(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build())!;
    }

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        return Task.FromResult<AuthorizationPolicy?>(null);
    }

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (String.IsNullOrWhiteSpace(policyName))
            return Task.FromResult<AuthorizationPolicy>(null!)!;

        return Task.FromResult(new AuthorizationPolicyBuilder()
            .AddRequirements(new PermissionAttribute(policyName))
            .Build())!;
    }
}
