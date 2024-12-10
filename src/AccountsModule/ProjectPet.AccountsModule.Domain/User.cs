using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using ProjectPet.AccountsModule.Domain.Accounts;
using ProjectPet.AccountsModule.Domain.UserData;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.AccountsModule.Domain;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public class User : IdentityUser<Guid>
{
    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;
    private List<SocialNetwork> _socialNetworks;
    public IReadOnlyList<Role> Roles => _roles;
    private List<Role> _roles;
    public VolunteerAccount VolunteerData { get; private set; }
    public AdminAccount AdminData { get; private set; }
    public MemberAccount MemberData { get; private set; }

    public User() { } //efcore

    public User(
        string username,
        string email,
        MemberAccount? memberData = null,
        AdminAccount? adminData = null,
        VolunteerAccount? volunteerData = null)
    {
        UserName = username;
        Email = email;

        VolunteerData = volunteerData!;
        MemberData = memberData!;
        AdminData = adminData!;
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public void UpdatePaymentMethods(IEnumerable<PaymentInfo> infos)
    {
        if (VolunteerData is null)
            VolunteerData = new VolunteerAccount(infos.ToList(), 0, []);

        VolunteerData = new VolunteerAccount(infos.ToList(), VolunteerData.Experience, VolunteerData.Certifications);
    }

    public void UpdateSocialNetworks(IEnumerable<SocialNetwork> networks)
    {
        _socialNetworks = networks.ToList();
    }

    #region Roled Users Factory Methods
    public static async Task<Result<User, Error[]>> CreateAdminAsync(
        UserManager<User> userManager,
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

        return await CreateUserAndAddRoleAsync(userManager, password, user, AdminAccount.ROLENAME);
    }


    public static async Task<Result<User, Error[]>> CreateMemberAsync(
        UserManager<User> userManager,
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

        return await CreateUserAndAddRoleAsync(userManager, password, user, MemberAccount.ROLENAME);
    }

    private static async Task<Result<User, Error[]>> CreateUserAndAddRoleAsync(
        UserManager<User> userManager,
        string password,
        User user,
        string roleName)
    {
        var createRes = await userManager.CreateAsync(user, password);
        if (createRes.Succeeded == false)
            return createRes.Errors.Select(x => Error.Failure(x.Code, x.Description)).ToArray();

        var addRoleRes = await userManager.AddToRoleAsync(user, roleName);
        if (addRoleRes.Succeeded == false)
            return addRoleRes.Errors.Select(x => Error.Failure(x.Code, x.Description)).ToArray();

        return user;
    }
    #endregion
}