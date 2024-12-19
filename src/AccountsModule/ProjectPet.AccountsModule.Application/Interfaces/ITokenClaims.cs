using CSharpFunctionalExtensions;
using ProjectPet.SharedKernel.ErrorClasses;
using System.Security.Claims;

namespace ProjectPet.AccountsModule.Application.Interfaces;

public interface ITokenClaims
{
    Task<Result<IReadOnlyList<Claim>, Error>> GetTokenClaims(string accessToken, CancellationToken cancellationToken = default);
}