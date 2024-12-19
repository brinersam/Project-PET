﻿using CSharpFunctionalExtensions;
using ProjectPet.AccountsModule.Application.Services;
using ProjectPet.AccountsModule.Contracts.Dto;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.AccountsModule.Application.Interfaces;
public interface ITokenProvider
{
    AccessTokenWJti GenerateJwtAccessToken(User user);
    Task<Guid> GenerateRefreshTokenAsync(User user, Guid Jti, CancellationToken cancellationToken = default);
    Task<Result<AuthTokensDto, Error>> GenerateTokenPairAsync(User user, CancellationToken cancellationToken);
}