using ProjectPet.SharedKernel.Dto;
using ProjectPet.Application.UseCases.Volunteers.Commands.CreateVolunteer;
using ProjectPet.Core.Abstractions;

namespace ProjectPet.VolunteerModule.Presentation.Volunteer.Requests;

public record CreateVolunteerRequest(
    CreateVolunteerDto VolunteerDto,
    List<PaymentInfoDto>? PaymentInfoDtos,
    List<SocialNetworkDto>? SocialNetworkDtos) : IToCommand<CreateVolunteerCommand>
{
    public CreateVolunteerCommand ToCommand()
    {
        return new CreateVolunteerCommand(
            VolunteerDto,
            PaymentInfoDtos,
            SocialNetworkDtos);
    }
}
