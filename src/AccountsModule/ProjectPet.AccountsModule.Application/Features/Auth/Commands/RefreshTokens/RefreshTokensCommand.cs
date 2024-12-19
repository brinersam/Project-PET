using ProjectPet.AccountsModule.Contracts.Dto;
using ProjectPet.Core.Abstractions;

namespace ProjectPet.AccountsModule.Presentation;

public record RefreshTokensCommand(AuthTokensDto Tokens)
    : IMapFromRequest<RefreshTokensCommand, RefreshTokensRequest>
{
    public static RefreshTokensCommand FromRequest(RefreshTokensRequest request)
        => new(request.Tokens);
}

