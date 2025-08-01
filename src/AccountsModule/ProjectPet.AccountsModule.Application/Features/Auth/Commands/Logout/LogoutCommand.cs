namespace ProjectPet.AccountsModule.Application.Features.Auth.Commands.Login;

public record LogoutCommand(Guid RefreshToken)
{
}