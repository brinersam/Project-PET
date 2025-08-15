using CSharpFunctionalExtensions;
using ProjectPet.AccountsModule.Application.Interfaces;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.AccountsModule.Application.Features.Auth.Commands.Logout;

public class LogoutHandler
{
    private readonly IAuthRepository _authRepository;

    public LogoutHandler(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    public async Task<UnitResult<Error>> HandleAsync(LogoutCommand cmd, CancellationToken cancellationToken = default)
    {
        await _authRepository.DeleteSessionAsync(cmd.RefreshToken, cancellationToken);
        return Result.Success<Error>();
    }
}
