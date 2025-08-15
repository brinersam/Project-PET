using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ProjectPet.AccountsModule.Application.Interfaces;
using ProjectPet.AccountsModule.Domain;
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
        if (userScopedData.IsSuccess == false)
        {
            context.Fail();
            return;
        }

        bool isRoleAuthorized = await _authRepository.DoesUserHavePermissionCodeAsync((Guid)userScopedData.UserId!, requirement.Code);
        if (isRoleAuthorized)
            context.Succeed(requirement);
    }
}
