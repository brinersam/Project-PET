using Microsoft.AspNetCore.Authorization;
using ProjectPet.Framework.Authorization;

namespace DEVShared.SecretKeyAuthentication;

public class SecretKeyAuthorizationHandler : AuthorizationHandler<PermissionAttribute> 
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionAttribute permission)
    {
        if (context.User.HasClaim(c => c is { Type: "IsService", Value: "true" }))
            context.Succeed(permission);

        return Task.CompletedTask;
    }
}