using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.Application.Extensions;
using ProjectPet.Domain.Models;
using ProjectPet.Domain.Shared;
using ProjectPet.VolunteerModule.Application.Interfaces;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteer.Commands.CreatePet;

public class CreatePetHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<CreatePetHandler> _logger;

    public CreatePetHandler(
        IVolunteerRepository volunteerRepository,
        ISpeciesRepository speciesRepository,
        IReadDbContext readDbContext,
        ILogger<CreatePetHandler> logger)
    {
        _volunteerRepository = volunteerRepository;
        _speciesRepository = speciesRepository;
        _readDbContext = readDbContext;
        _logger = logger;
    }
    public async Task<Result<Guid, Error>> HandleAsync(
        CreatePetCommand command,
        CancellationToken cancellationToken)
    {
        var doesSpeciesExist = _readDbContext.Species.Any(x => x.Id == command.AnimalData.SpeciesId);
        if (doesSpeciesExist == false)
            return Error.Validation("record.not.found", $"Species with id \"{command.AnimalData.SpeciesId}\" was not found!");

        var breed = _readDbContext.Breeds.FirstOrDefault(x => x.SpeciesId == command.AnimalData.SpeciesId && x.Value == command.AnimalData.BreedName);
        if (breed is null)
            return Error.Validation("record.not.found", $"Breed with name \"{command.AnimalData.BreedName}\" was not found!");

        var volunteerRes = await _volunteerRepository.GetByIdAsync(command.Id, cancellationToken);
        if (volunteerRes.IsFailure)
            return volunteerRes.Error;
        var volunteer = volunteerRes.Value;

        var orderingPosition = Position.Create(volunteer.OwnedPets.Count() + 1);

        var animalData = AnimalData.Create(command.AnimalData.SpeciesId, breed.Id).Value;

        List<PaymentInfo> paymentInfos = command.PaymentInfos
            .Select(x =>
                PaymentInfo.Create(x.Title, x.Instructions).Value)
            .ToList();

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

        if (Ext.IsDelegateFailed(out Pet pet, out var petError,
            Pet.Create(
                command.Name,
                animalData,
                command.Description,
                command.Coat,
                orderingPosition,
                healthInfo,
                address,
                phoneNumber,
                command.Status,
                command.DateOfBirth,
                [],
                paymentInfos)
        ))
            return petError!;

        volunteer.AddPet(pet);
        await _volunteerRepository.Save(volunteer, cancellationToken);

        return pet.Id;
    }
}
