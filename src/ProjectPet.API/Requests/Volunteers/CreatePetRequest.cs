using ProjectPet.API.Etc;
using ProjectPet.Application.Dto;
using ProjectPet.Application.UseCases.Volunteers.Commands.CreatePet;
using ProjectPet.Domain.Models;

namespace ProjectPet.API.Requests.Volunteers;
public record CreatePetRequest(
    string Name,
    string Coat,
    string Description,
    DateTime DateOfBirth,
    AnimalDataDto AnimalData,
    HealthInfoDto HealthInfo,
    List<PaymentInfoDto> PaymentInfos,
    AddressDto Address,
    PhonenumberDto Phonenumber,
    PetStatus Status = PetStatus.NotSet) : IToCommand<CreatePetCommand, Guid>
{
    public CreatePetCommand ToCommand(Guid id)
    {
        return new CreatePetCommand(
            id,
            Name,
            Coat,
            Description,
            DateOnly.FromDateTime(DateOfBirth),
            AnimalData,
            HealthInfo,
            PaymentInfos,
            Address,
            Phonenumber,
            Status);
    }
}
