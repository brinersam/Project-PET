using ProjectPet.AccountsModule.Contracts.Dto;
using ProjectPet.AccountsModule.Contracts.Requests;
using ProjectPet.Core.Abstractions;

namespace ProjectPet.AccountsModule.Application.Features.Auth.Commands.RefreshTokens;

public record RefreshTokensCommand(AuthTokensDto Tokens)
    : IMapFromRequest<RefreshTokensCommand, RefreshTokensRequest>
{
    public static RefreshTokensCommand FromRequest(RefreshTokensRequest request)
        => new(request.Tokens);
}

