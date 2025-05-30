using ProjectPet.AccountsModule.Contracts.Requests;
using ProjectPet.Core.Requests;

namespace ProjectPet.AccountsModule.Application.Features.Auth.Commands.Register;

public record RegisterCommand(string Email, string Username, string Password)
    : IMapFromRequest<RegisterCommand, RegisterRequest>
{
    public static RegisterCommand FromRequest(RegisterRequest request)
        => new(request.Email, request.Username, request.Password);
}
