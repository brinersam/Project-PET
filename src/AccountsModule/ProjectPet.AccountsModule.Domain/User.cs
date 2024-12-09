using Microsoft.AspNetCore.Identity;
using ProjectPet.AccountsModule.Domain.Accounts;
using ProjectPet.AccountsModule.Domain.UserData;

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
}