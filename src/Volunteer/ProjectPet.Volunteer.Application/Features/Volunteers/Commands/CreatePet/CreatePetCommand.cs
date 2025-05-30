using ProjectPet.Core.Requests;
using ProjectPet.SharedKernel.SharedDto;
using ProjectPet.VolunteerModule.Contracts.Dto;
using ProjectPet.VolunteerModule.Contracts.Requests;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.CreatePet;

public record CreatePetCommand(
    Guid Id,
    string Name,
    string Coat,
    string Description,
    DateOnly DateOfBirth,
    AnimalDataDto AnimalData,
    HealthInfoDto HealthInfo,
    List<PaymentInfoDto> PaymentInfos,
    AddressDto Address,
    PhonenumberDto PhoneNumber,
    PetStatusDto Status = PetStatusDto.NotSet) : IMapFromRequest<CreatePetCommand, CreatePetRequest, Guid>
{
    public static CreatePetCommand FromRequest(CreatePetRequest req, Guid id)
    {
        return new CreatePetCommand(
            id,
            req.Name,
            req.Coat,
            req.Description,
            DateOnly.FromDateTime(req.DateOfBirth),
            req.AnimalData,
            req.HealthInfo,
            req.PaymentInfos,
            req.Address,
            req.Phonenumber,
            req.Status);
    }
}
