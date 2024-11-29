using Microsoft.AspNetCore.Identity;
using ProjectPet.AccountsModule.Domain.Accounts;
using ProjectPet.AccountsModule.Domain.UserData;

namespace ProjectPet.AccountsModule.Domain;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public class User : IdentityUser<Guid>
{
    public Guid RoleId { get; private set; }
    public IReadOnlyList<PaymentInfo> PaymentInfos => _paymentInfos;
    private List<PaymentInfo> _paymentInfos;
    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;
    private List<SocialNetwork> _socialNetworks;
    public VolunteerAccount VolunteerData { get; private set; }
    public AdminAccount AdminData { get; private set; }
    public MemberAccount MemberData { get; private set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public User(){} //efcore

    public void UpdatePaymentMethods(IEnumerable<PaymentInfo> infos)
    {
        _paymentInfos = infos.ToList();
    }

    public void UpdateSocialNetworks(IEnumerable<SocialNetwork> networks)
    {
        _socialNetworks = networks.ToList();
    }
}