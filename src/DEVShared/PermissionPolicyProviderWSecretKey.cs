using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using ProjectPet.Framework.Authorization;

namespace DEVShared;
public class PermissionPolicyProviderWSecretKey : IAuthorizationPolicyProvider
{
    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        return Task.FromResult
        (
             new AuthorizationPolicyBuilder
                 (
                    JwtBearerDefaults.AuthenticationScheme,
                    "SecretKey"
                 )
                 .RequireAuthenticatedUser()
                 .Build()
        );
    }

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        return Task.FromResult<AuthorizationPolicy>(null);
    }

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (string.IsNullOrWhiteSpace(policyName))
            return Task.FromResult<AuthorizationPolicy>(null);

        var policy = new AuthorizationPolicyBuilder
            (
                JwtBearerDefaults.AuthenticationScheme,
                "SecretKey"
            )
            .RequireAuthenticatedUser()
            .AddRequirements(new PermissionAttribute(policyName))
            .Build();

        return Task.FromResult<AuthorizationPolicy?>(policy);
    }
}