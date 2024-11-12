using ProjectPet.Application.Dto;

namespace ProjectPet.Application.UseCases.Volunteers.Commands.UpdateVolunteerSocials;

public record UpdateVolunteerSocialsCommand(
    Guid Id,
    List<SocialNetworkDto> SocialNetworks);
