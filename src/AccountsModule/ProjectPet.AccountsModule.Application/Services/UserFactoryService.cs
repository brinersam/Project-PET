using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.AccountsModule.Domain.Accounts;
using ProjectPet.AccountsModule.Domain.UserData;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.AccountsModule.Application.Services;
public class UserFactoryService
{
    private readonly UserManager<User> _userManager;

    public UserFactoryService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<User,Error[]>> CreateAdminUserAsync(
        string username,
        string password,
        string email,
        AdminAccount roleData)
    {
        User user = new User
        (
            username,
            email,
            adminData: roleData
        );

        return await CreateUserAndAddRoleAsync(password, user, AdminAccount.ROLENAME);
    }


    public async Task<Result<User, Error[]>> CreateMemberUserAsync(
        string username,
        string password,
        string email,
        MemberAccount roleData)
    {
        User user = new User
        (
            username,
            email,
            memberData: roleData
        );

        return await CreateUserAndAddRoleAsync(password, user, MemberAccount.ROLENAME);
    }
    private async Task<Result<User, Error[]>> CreateUserAndAddRoleAsync(string password, User user, string roleName)
    {
        var createRes = await _userManager.CreateAsync(user, password);
        if (createRes.Succeeded == false)
            return createRes.Errors.Select(x => Error.Failure(x.Code, x.Description)).ToArray();

        var addRoleRes = await _userManager.AddToRoleAsync(user, roleName);
        if (addRoleRes.Succeeded == false)
            return addRoleRes.Errors.Select(x => Error.Failure(x.Code, x.Description)).ToArray();

        return user;
    }
}
