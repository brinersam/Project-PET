using CSharpFunctionalExtensions;
using ProjectPet.AccountsModule.Application.Interfaces;
using ProjectPet.AccountsModule.Contracts.Dto;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.AccountsModule.Application.Features.Auth.Commands.RefreshTokens;

public class RefreshTokensHandler
{
    private readonly ITokenRefresher _tokenRefresher;

    public RefreshTokensHandler(ITokenRefresher tokenRefresher)
    {
        _tokenRefresher = tokenRefresher;
    }

    public async Task<Result<LoginResponse, Error>> HandleAsync(RefreshTokensCommand cmd, CancellationToken cancellationToken = default)
    {
        var result = await _tokenRefresher
            .RefreshTokens(cmd.RefreshToken, cancellationToken);

        if (result.IsFailure)
            return result.Error;

        return result;
    }
}