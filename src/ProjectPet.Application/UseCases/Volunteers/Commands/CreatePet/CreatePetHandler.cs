using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.Application.Database;
using ProjectPet.Application.Extensions;
using ProjectPet.Application.Repositories;
using ProjectPet.Domain.Models;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Application.UseCases.Volunteers.Commands.CreatePet;

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
        var doesSpeciesExist = _readDbContext.Species.Any(x => x.Id == command.AnimalData_SpeciesId);
        if (doesSpeciesExist == false)
            return Error.Validation("record.not.found", $"Species with id \"{command.AnimalData_SpeciesId}\" was not found!");

        var breed = _readDbContext.Breeds.FirstOrDefault(x => x.SpeciesId == command.AnimalData_SpeciesId && x.Value == command.AnimalData_BreedName);
        if (breed is null)
            return Error.Validation("record.not.found", $"Breed with name \"{command.AnimalData_BreedName}\" was not found!");

        if (Ext.IsDelegateFailed(out AnimalData animalData, out var animalDataError,
            AnimalData.Create(
                command.AnimalData_SpeciesId,
                breed.Id)
        ))
            return animalDataError!;

        List<PaymentInfo> paymentInfos = [];
        foreach (var paymentInfoDto in command.PaymentInfos)
        {
            if (Ext.IsDelegateFailed(out PaymentInfo paymentInfo, out var paymentInfoError,
                PaymentInfo.Create(
                    paymentInfoDto.Title,
                    paymentInfoDto.Instructions)
            ))
                return paymentInfoError!;

            paymentInfos.Add(paymentInfo);
        }

        if (Ext.IsDelegateFailed(out Phonenumber phoneNumber, out var phoneNumberError,
            Phonenumber.Create(
                command.PhoneNumber.Phonenumber,
                command.PhoneNumber.PhonenumberAreaCode)
        ))
            return phoneNumberError!;

        if (Ext.IsDelegateFailed(out Address address, out var addressError,
            Address.Create(
                command.Address.Name,
                command.Address.Street,
                command.Address.Building,
                command.Address.Block,
                command.Address.Entrance,
                command.Address.Floor,
                command.Address.Apartment)
        ))
            return addressError!;

        if (Ext.IsDelegateFailed(out HealthInfo healthInfo, out var healthInfoError,
            HealthInfo.Create(
                command.HealthInfo.Health,
                command.HealthInfo.IsSterilized,
                command.HealthInfo.IsVaccinated,
                command.HealthInfo.Weight,
                command.HealthInfo.Height)
        ))
            return healthInfoError!;

        var volunteerRes = await _volunteerRepository.GetByIdAsync(command.Id, cancellationToken);
        if (volunteerRes.IsFailure)
            return volunteerRes.Error;
        var volunteer = volunteerRes.Value;

        var orderingPosition = Position.Create(volunteer.OwnedPets.Count() + 1);

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
