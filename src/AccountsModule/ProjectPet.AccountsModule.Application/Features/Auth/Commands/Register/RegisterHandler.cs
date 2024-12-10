using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.AccountsModule.Domain.Accounts;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.AccountsModule.Application.Features.Auth.Commands.Register;

public class RegisterHandler
{
    private readonly UserManager<User> _userManager;

    public RegisterHandler(
        UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UnitResult<Error[]>> HandleAsync(RegisterCommand cmd, CancellationToken cancellationToken)
    {
        var userWithEmail = await _userManager.FindByEmailAsync(cmd.Email);
        if (userWithEmail != null)
            return new Error[] { Error.Validation("user.alrady.exists", $"Can't register an account with email {cmd.Email}") };

        var createUserResult = await User.CreateMemberAsync(_userManager, cmd.Username, cmd.Password, cmd.Email, new MemberAccount());
        if (createUserResult.IsFailure)
            return createUserResult.Error;

        return Result.Success<Error[]>();
    }
}
