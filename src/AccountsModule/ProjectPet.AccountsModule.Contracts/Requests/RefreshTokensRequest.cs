using ProjectPet.AccountsModule.Contracts.Dto;

namespace ProjectPet.AccountsModule.Contracts.Requests;

public record RefreshTokensRequest(AuthTokensDto Tokens);
