#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using ProjectPet.SharedKernel.SharedDto;

namespace ProjectPet.AccountsModule.Contracts.Dto;

public class VolunteerAccountDto
{
    public List<PaymentInfoDto> PaymentInfos;
    public int Experience { get; init; }
    public string[] Certifications { get; init; }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.