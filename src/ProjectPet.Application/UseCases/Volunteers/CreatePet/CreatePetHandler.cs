using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.Application.Extensions;
using ProjectPet.Application.UseCases.AnimalSpecies;
using ProjectPet.Domain.Models;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Application.UseCases.Volunteers.CreatePet;

public class CreatePetHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<CreatePetHandler> _logger;

    public CreatePetHandler(
        IVolunteerRepository volunteerRepository,
        ISpeciesRepository speciesRepository,
        ILogger<CreatePetHandler> logger)
    {
        _volunteerRepository = volunteerRepository;
        _speciesRepository = speciesRepository;
        _logger = logger;
    }
    public async Task<Result<Guid, Error>> HandleAsync(
        CreatePetCommand command,
        CancellationToken cancellationToken)
    {
        var speciesRes = await _speciesRepository.GetByIdAsync(
            command.AnimalData_SpeciesId,
            cancellationToken);
        if (speciesRes.IsFailure)
            return speciesRes.Error;

        if (speciesRes.Value.TryFindBreedByName(command.AnimalData_BreedName, out Breed breed) == false)
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

        if (Ext.IsDelegateFailed(out PhoneNumber phoneNumber, out var phoneNumberError,
            PhoneNumber.Create(
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

        var orderingPosition = Position.Create(volunteer.OwnedPets.Count()+1);

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
