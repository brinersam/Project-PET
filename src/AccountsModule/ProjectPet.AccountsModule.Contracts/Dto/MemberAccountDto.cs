#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace ProjectPet.AccountsModule.Contracts.Dto;

public class MemberAccountDto
{
    public List<Guid> FavoritePetIds { get; set; }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.