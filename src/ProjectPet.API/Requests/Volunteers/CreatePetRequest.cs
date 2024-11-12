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
    Guid AnimalData_SpeciesId,
    string AnimalData_BreedName,
    HealthInfoDto HealthInfo,
    List<PaymentInfoDto> PaymentInfos,
    AddressDto Address,
    PhoneNumberDto PhoneNumber,
    Status Status = Status.NotSet) : IToCommand<CreatePetCommand, Guid>
{
    public CreatePetCommand ToCommand(Guid id)
    {
        return new CreatePetCommand(
            id,
            Name,
            Coat,
            Description,
            DateOnly.FromDateTime(DateOfBirth),
            AnimalData_SpeciesId,
            AnimalData_BreedName,
            HealthInfo,
            PaymentInfos,
            Address,
            PhoneNumber,
            Status);
    }
}
