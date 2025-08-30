using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ProjectPet.AccountsModule.Application.Interfaces;
using ProjectPet.Framework.Authorization;

namespace ProjectPet.AccountsModule.Application.Services;

public class PermissionRequirementHandler : AuthorizationHandler<PermissionAttribute>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthRepository _authRepository;

    public PermissionRequirementHandler(
        IHttpContextAccessor httpContextAccessor,
        IAuthRepository authRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _authRepository = authRepository;
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
        if (userScopedData.UserId is null || userScopedData.IsSuccess == false)
        {
            context.Fail();
            return;
        }

        bool isRoleAuthorized = false;

        try
        {
            isRoleAuthorized = await _authRepository.DoesUserHavePermissionCodeAsync((Guid)userScopedData.UserId!, requirement.Code);
        }
        catch (InvalidOperationException ex) // no db, verify using whatever is in jwt
        {
            isRoleAuthorized = _authRepository.DoesUserHavePermissionCode(userScopedData, requirement.Code);
        }

        if (isRoleAuthorized)
            context.Succeed(requirement);
    }
}
