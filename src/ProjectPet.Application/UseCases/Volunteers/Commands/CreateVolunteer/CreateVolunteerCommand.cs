using ProjectPet.Application.Dto;

namespace ProjectPet.Application.UseCases.Volunteers.Commands.CreateVolunteer;

public record CreateVolunteerCommand(
    VolunteerDto VolunteerDto,
    PhoneNumberDto PhonenumberDto,
    List<PaymentInfoDto>? PaymentInfoDtos,
    List<SocialNetworkDto>? SocialNetworkDtos);
