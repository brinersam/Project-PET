using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ProjectPet.AccountsModule.Application.Interfaces;
using ProjectPet.Framework.Authorization;

namespace ProjectPet.AccountsModule.Application.Services;

public class PermissionRequirementHandler : AuthorizationHandler<PermissionAttribute>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PermissionRequirementHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionAttribute requirement)
    {
        if (context.User.Identity?.IsAuthenticated == false || _httpContextAccessor.HttpContext is null)
            return;

        var userScopedData = _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<UserScopedData>();
        if (userScopedData.UserId is null || userScopedData.IsSuccess == false)
            return;

        if (DoesUserHavePermissionCode(userScopedData, requirement.Code))
            context.Succeed(requirement);
    }

    private bool DoesUserHavePermissionCode(UserScopedData userData, string permissionCode)
    {
        var permissionExists = userData.Permissions.Any
            (
                permission => Equals(permission, permissionCode) || // rolePermission has the required permission code
                              Equals(permission, "admin.masterkey") // rolePermission is admin sudo),
            );

        return permissionExists;
    }
}
