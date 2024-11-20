using ProjectPet.Application.Dto;

namespace ProjectPet.VolunteerModule.Presentation.Volunteer.Requests;

public record class UpdateVolunteerSocialsRequest(List<SocialNetworkDto> SocialNetworks);
