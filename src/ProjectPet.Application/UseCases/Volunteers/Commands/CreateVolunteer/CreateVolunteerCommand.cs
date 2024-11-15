using ProjectPet.Application.Dto;

namespace ProjectPet.Application.UseCases.Volunteers.Commands.CreateVolunteer;

public record CreateVolunteerCommand(
    CreateVolunteerDto VolunteerDto,
    List<PaymentInfoDto>? PaymentInfoDtos,
    List<SocialNetworkDto>? SocialNetworkDtos);
