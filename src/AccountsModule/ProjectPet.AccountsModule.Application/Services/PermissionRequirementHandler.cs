using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using ProjectPet.AccountsModule.Application.Interfaces;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.Framework.Authorization;
using ProjectPet.SharedKernel.ErrorClasses;

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

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionAttribute requirement)
    {
        var claim = context.User.Claims.FirstOrDefault(c => c.Type == "Role");
        if (claim == null)
            return Task.CompletedTask;

        Result<Permission, Error> permRes = _authRepository.GetPermissionsForRole(new Guid(claim.Value));
        if (permRes.IsFailure)
        {
            _logger.LogWarning($"{nameof(PermissionRequirementHandler)} module tried to get a non existing role by id: {claim.Value} !");
            return Task.CompletedTask;
        }
        var perm = permRes.Value;

        if (perm.Code.Contains(requirement.Code))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
