using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.SpeciesModule.Contracts;
using ProjectPet.VolunteerModule.Application.Interfaces;
using ProjectPet.VolunteerModule.Domain.Models;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.CreatePet;

public class CreatePetHandler
{
    private readonly ISpeciesContract _speciesContract;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<CreatePetHandler> _logger;

    public CreatePetHandler(
        ISpeciesContract speciesContract,
        IVolunteerRepository volunteerRepository,
        IReadDbContext readDbContext,
        ILogger<CreatePetHandler> logger)
    {
        _speciesContract = speciesContract;
        _volunteerRepository = volunteerRepository;
        _readDbContext = readDbContext;
        _logger = logger;
    }
    public async Task<Result<Guid, Error>> HandleAsync(
        CreatePetCommand command,
        CancellationToken cancellationToken)
    {
        var speciesRes = await _speciesContract.GetSpeciesByIdAsync(command.AnimalData.SpeciesId, cancellationToken);
        if (speciesRes.IsFailure)
            return Error.Validation("record.not.found", $"Species with id \"{command.AnimalData.SpeciesId}\" was not found!");
        var species = speciesRes.Value;

        var breedRes = await _speciesContract.GetBreedByNameAsync(command.AnimalData.BreedName, cancellationToken);
        if (breedRes.IsFailure)
            return Error.Validation("record.not.found", $"Breed with name \"{command.AnimalData.BreedName}\" was not found!");
        var breed = breedRes.Value;

        var volunteerRes = await _volunteerRepository.GetByIdAsync(command.Id, cancellationToken);
        if (volunteerRes.IsFailure)
            return volunteerRes.Error;
        var volunteer = volunteerRes.Value;

        var orderingPosition = Position.Create(volunteer.OwnedPets.Count + 1);

        var animalData = AnimalData.Create(command.AnimalData.SpeciesId, breed.Id).Value;

        List<PaymentInfo> paymentInfos = command.PaymentInfos
            .Select(x =>
                PaymentInfo.Create(x.Title, x.Instructions).Value)
            .ToList();

        var status = (PetStatus)command.Status;
        if (Enum.IsDefined(typeof(PetStatus), status) == false)
        {
            return Error.Validation("invalid.value", $"Invalid status: \"{status}\"");
        }

        var phoneNumber = Phonenumber.Create(
                command.PhoneNumber.Phonenumber,
                command.PhoneNumber.PhonenumberAreaCode).Value;

        var address = Address.Create(
                command.Address.Name,
                command.Address.Street,
                command.Address.Building,
                command.Address.Block,
                command.Address.Entrance,
                command.Address.Floor,
                command.Address.Apartment).Value;

        var healthInfo = HealthInfo.Create(
                command.HealthInfo.Health,
                command.HealthInfo.IsSterilized,
                command.HealthInfo.IsVaccinated,
                command.HealthInfo.Weight,
                command.HealthInfo.Height).Value;

        var petCreateRes = Pet.Create(
                command.Name,
                animalData,
                command.Description,
                command.Coat,
                orderingPosition,
                healthInfo,
                address,
                phoneNumber,
                status,
                command.DateOfBirth,
                [],
                paymentInfos);
        if (petCreateRes.IsFailure)
            return petCreateRes.Error;

        var pet = petCreateRes.Value;

        volunteer.AddPet(pet);
        await _volunteerRepository.Save(volunteer, cancellationToken);

        return pet.Id;
    }
}
