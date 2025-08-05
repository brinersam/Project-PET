using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using ProjectPet.AccountsModule.Application.Interfaces;
using ProjectPet.AccountsModule.Contracts.Dto;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.AccountsModule.Application.Features.Auth.Commands.RefreshTokens;

public class RefreshTokensHandler
{
    private readonly ITokenRefresher _tokenRefresher;
    private readonly IAuthRepository _authRepository;
    private readonly UserManager<User> _userManager;

    public RefreshTokensHandler(
        ITokenRefresher tokenRefresher,
        IAuthRepository authRepository,
        UserManager<User> userManager)
    {
        _tokenRefresher = tokenRefresher;
        _authRepository = authRepository;
        _userManager = userManager;
    }

    public async Task<Result<LoginResponse, Error>> HandleAsync(RefreshTokensCommand cmd, CancellationToken cancellationToken = default)
    {
        var session = await _authRepository.GetRefreshSessionAsync(cmd.RefreshToken, cancellationToken);
        if (session is null)
            return Errors.General.NotFound(typeof(RefreshSession));

        var result = await _tokenRefresher
            .RefreshTokens(cmd.RefreshToken, cancellationToken);

        if (result.IsFailure)
            return result.Error;

        var userRoles = await _userManager.GetRolesAsync(session.User);

        return Result.Success<LoginResponse, Error>(result.Value with {Roles = userRoles.ToList()});
    }
}