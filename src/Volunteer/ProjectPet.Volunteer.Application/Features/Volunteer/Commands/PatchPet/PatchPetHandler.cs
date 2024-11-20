using CSharpFunctionalExtensions;
using ProjectPet.Domain.Models;
using ProjectPet.Domain.Shared;
using ProjectPet.VolunteerModule.Application.Interfaces;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteer.Commands.PatchPet;

public class PatchPetHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IReadDbContext _readDbContext;

    public PatchPetHandler(
        IVolunteerRepository volunteerRepository,
        IReadDbContext readDbContext)
    {
        _volunteerRepository = volunteerRepository;
        _readDbContext = readDbContext;
    }

    public async Task<UnitResult<Error>> HandleAsync(PatchPetCommand command, CancellationToken cancellationToken)
    {
        var volunteerRes = await _volunteerRepository.GetByIdAsync(command.VolunteerId, cancellationToken);
        if (volunteerRes.IsFailure)
            return volunteerRes.Error;
        var volunteer = volunteerRes.Value;

        AnimalData? animalData = null;
        if (command.AnimalData != null)
        {
            var doesSpeciesExist = _readDbContext.Species.Any(x => x.Id == command.AnimalData.SpeciesId);
            if (doesSpeciesExist == false)
                return Error.Validation("record.not.found", $"Species with id \"{command.AnimalData.SpeciesId}\" was not found!");

            var breed = _readDbContext.Breeds.FirstOrDefault(x => x.SpeciesId == command.AnimalData.SpeciesId && x.Value == command.AnimalData.BreedName);
            if (breed is null)
                return Error.Validation("record.not.found", $"Breed with name \"{command.AnimalData.BreedName}\" was not found!");

            animalData = AnimalData.Create(command.AnimalData.SpeciesId, breed.Id).Value;
        }

        HealthInfo? healthInfo = null;
        if (command.HealthInfo != null)
        {
            healthInfo = HealthInfo.Create(
                command.HealthInfo.Health,
                command.HealthInfo.IsSterilized,
                command.HealthInfo.IsVaccinated,
                command.HealthInfo.Weight,
                command.HealthInfo.Height).Value;
        }

        Address? address = null;
        if (command.Address != null)
        {
            address = Address.Create(
                command.Address.Name,
                command.Address.Street,
                command.Address.Building,
                command.Address.Block,
                command.Address.Entrance,
                command.Address.Floor,
                command.Address.Apartment).Value;
        }

        Phonenumber? phonenumber = null;
        if (command.Phonenumber != null)
        {
            phonenumber = Phonenumber.Create(
                command.Phonenumber.Phonenumber,
                command.Phonenumber.PhonenumberAreaCode).Value;
        }

        var result = volunteer.PatchPet(
            command.Petid,
            command.Name,
            animalData,
            command.Description,
            command.Coat,
            healthInfo,
            address,
            phonenumber);

        if (result.IsFailure)
            return result.Error;

        await _volunteerRepository.Save(volunteer, cancellationToken);

        return Result.Success<Error>();
    }
}
