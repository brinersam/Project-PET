#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using ProjectPet.SharedKernel.SharedDto;

namespace ProjectPet.AccountsModule.Contracts.Dto;
public class UserDto
{
    public Guid Id { get; set; }
    public IReadOnlyList<SocialNetworkDto> SocialNetworks { get; set; }
    //public List<string> Roles { get; set; }
    public VolunteerAccountDto VolunteerData { get; set; }
    public AdminAccountDto AdminData { get; set; }
    public MemberAccountDto MemberData { get; set; }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.