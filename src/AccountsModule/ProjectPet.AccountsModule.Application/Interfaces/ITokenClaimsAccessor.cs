using CSharpFunctionalExtensions;
using ProjectPet.SharedKernel.ErrorClasses;
using System.Security.Claims;

namespace ProjectPet.AccountsModule.Application.Interfaces;

public interface ITokenClaimsAccessor
{
    Task<Result<IReadOnlyList<Claim>, Error>> GetTokenClaims(string accessToken, CancellationToken cancellationToken = default);
}