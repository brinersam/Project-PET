using ProjectPet.Application.Dto;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteer.Commands.UpdateVolunteerSocials;

public record UpdateVolunteerSocialsCommand(
    Guid Id,
    List<SocialNetworkDto> SocialNetworks);
