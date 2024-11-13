using ProjectPet.API.Etc;
using ProjectPet.Application.Dto;
using ProjectPet.Application.UseCases.Volunteers.Commands.CreateVolunteer;

namespace ProjectPet.API.Requests.Volunteers;

public record CreateVolunteerRequest(
    VolunteerDto VolunteerDto,
    PhoneNumberDto PhonenumberDto,
    List<PaymentInfoDto>? PaymentInfoDtos,
    List<SocialNetworkDto>? SocialNetworkDtos) : IToCommand<CreateVolunteerCommand>
{
    public CreateVolunteerCommand ToCommand()
    {
        return new CreateVolunteerCommand(
            VolunteerDto,
            PhonenumberDto,
            PaymentInfoDtos,
            SocialNetworkDtos);
    }
}
