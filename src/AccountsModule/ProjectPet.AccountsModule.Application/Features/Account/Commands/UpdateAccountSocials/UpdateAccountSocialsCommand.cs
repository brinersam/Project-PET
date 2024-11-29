using ProjectPet.AccountsModule.Contracts.Dto;
using ProjectPet.AccountsModule.Contracts.Requests;
using ProjectPet.Core.Abstractions;

namespace ProjectPet.AccountsModule.Application.Features.Account.Commands.UpdateAccountSocials;

public record UpdateAccountSocialsCommand(List<SocialNetworkDto> SocialNetworks, Guid UserId)
    : IMapFromRequest<UpdateAccountSocialsCommand, UpdateAccountSocialsRequest, Guid>
{
    public static UpdateAccountSocialsCommand FromRequest(UpdateAccountSocialsRequest req, Guid userId)
        => new UpdateAccountSocialsCommand(req.SocialNetworks, userId);
}
