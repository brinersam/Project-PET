using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.AccountsModule.Application.Features.Auth.Commands.Register;

public class RegisterHandler
{
    private readonly UserManager<User> _userManager;

    public RegisterHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UnitResult<Error[]>> HandleAsync(RegisterCommand cmd, CancellationToken cancellationToken)
    {
        var userWithEmail = await _userManager.FindByEmailAsync(cmd.Email);
        if (userWithEmail != null)
            return new Error[] {Error.Validation("user.alrady.exists", $"Can't register an account with email {cmd.Email}")};

        var user = new User() 
        {
            UserName = cmd.Username,
            Email = cmd.Email,
        };

        var createRes = await _userManager.CreateAsync(user, cmd.Password);
        if (createRes.Succeeded == false)
            return createRes.Errors.Select(x => Error.Failure(x.Code, x.Description)).ToArray();

        var addRoleRes = await _userManager.AddToRoleAsync(user, "Member");
        if (addRoleRes.Succeeded == false)
            return addRoleRes.Errors.Select(x => Error.Failure(x.Code, x.Description)).ToArray();

        return Result.Success<Error[]>();
    }
}
