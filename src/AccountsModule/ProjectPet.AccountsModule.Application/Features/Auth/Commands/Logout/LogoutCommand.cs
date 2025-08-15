namespace ProjectPet.AccountsModule.Application.Features.Auth.Commands.Logout;

public record LogoutCommand(Guid RefreshToken)
{
}