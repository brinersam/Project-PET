using ProjectPet.Core.Abstractions;
using ProjectPet.VolunteerModule.Contracts.Dto;
using ProjectPet.VolunteerModule.Contracts.Requests;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.UpdateVolunteerSocials;

public record UpdateVolunteerSocialsCommand(
    Guid Id,
    List<SocialNetworkDto> SocialNetworks) : IMapFromRequest<UpdateVolunteerSocialsCommand, UpdateVolunteerSocialsRequest, Guid>
{
    public static UpdateVolunteerSocialsCommand FromRequest(UpdateVolunteerSocialsRequest req, Guid id)
    {
        return new UpdateVolunteerSocialsCommand(id, req.SocialNetworks);
    }
}
