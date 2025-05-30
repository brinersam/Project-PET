using ProjectPet.AccountsModule.Contracts.Requests;
using ProjectPet.Core.Requests;

namespace ProjectPet.AccountsModule.Application.Features.Auth.Commands.Login;

public record LoginCommand(string Email, string Password)
    : IMapFromRequest<LoginCommand, LoginRequest>
{
    public static LoginCommand FromRequest(LoginRequest request)
        => new(request.Email, request.Password);
}