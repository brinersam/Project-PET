using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ProjectPet.AccountsModule.Application.Interfaces;
using ProjectPet.Framework.Authorization;
using ProjectPet.SharedKernel.ErrorClasses;
using System.ComponentModel;

namespace ProjectPet.AccountsModule.Application.Services;

public class PermissionRequirementHandler : AuthorizationHandler<PermissionAttribute>
{
    private readonly IAuthRepository _authRepository;
    private readonly ILogger<PermissionRequirementHandler> _logger;

    public PermissionRequirementHandler(
        IAuthRepository authRepository,
        ILogger<PermissionRequirementHandler> logger)
    {
        _authRepository = authRepository;
        _logger = logger;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionAttribute requirement)
    {
        string? userId = context.User.Claims.FirstOrDefault(u => u.Properties.Values.Contains("sub"))?.Value;
        if (String.IsNullOrWhiteSpace(userId))
            throw new Exception($"{typeof(PermissionRequirementHandler)}: User has no id!");

        bool isRoleAuthorized = await _authRepository.DoesUserHavePermissionCodeAsync(new Guid(userId), requirement.Code);

        if (isRoleAuthorized)
            context.Succeed(requirement);
    }
}
