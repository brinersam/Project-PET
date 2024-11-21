using ProjectPet.SharedKernel.Dto;
using ProjectPet.Application.UseCases.Volunteers.Commands.CreatePet;
using ProjectPet.Core.Abstractions;
using ProjectPet.Domain.Models;

namespace ProjectPet.VolunteerModule.Presentation.Volunteer.Requests;
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
