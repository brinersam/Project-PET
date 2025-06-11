using ProjectPet.AccountsModule.Contracts.Requests;
namespace ProjectPet.AccountsModule.Application.Features.Auth.Commands.RefreshTokens;

public record RefreshTokensCommand(Guid RefreshToken)
{}

