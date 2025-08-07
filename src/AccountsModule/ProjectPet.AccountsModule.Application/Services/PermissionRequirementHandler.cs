using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ProjectPet.AccountsModule.Application.Interfaces;
using ProjectPet.Framework.Authorization;
using tempShared.Framework.Authorization;

namespace ProjectPet.AccountsModule.Application.Services;

public class PermissionRequirementHandler : AuthorizationHandler<PermissionAttribute>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PermissionRequirementHandler(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionAttribute requirement)
    {
        if (context.User.Identity?.IsAuthenticated == false || _httpContextAccessor.HttpContext is null)
        {
            context.Fail();
            return;
        }

        var userScopedData = _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<UserScopedData>();

        if (userScopedData.Permissions?.Contains(requirement.Code) == false)
        {
            context.Fail();
            return;
        }

        context.Succeed(requirement);
    }
}
