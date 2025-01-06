using CSharpFunctionalExtensions;
using ProjectPet.AccountsModule.Contracts.Dto;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.AccountsModule.Application.Interfaces;

public interface ITokenRefresher
{
    Task<Result<AuthTokensDto, Error>> RefreshTokens(string accessToken, Guid refreshToken, CancellationToken cancellationToken = default);
}