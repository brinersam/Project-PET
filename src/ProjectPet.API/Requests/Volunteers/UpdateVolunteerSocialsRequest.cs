using ProjectPet.Application.Dto;

namespace ProjectPet.API.Requests.Volunteers;

public record class UpdateVolunteerSocialsRequest(List<SocialNetworkDto> SocialNetworks);
