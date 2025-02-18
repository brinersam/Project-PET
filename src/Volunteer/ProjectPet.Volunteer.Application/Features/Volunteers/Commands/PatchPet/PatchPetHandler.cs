﻿using CSharpFunctionalExtensions;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.SpeciesModule.Contracts;
using ProjectPet.VolunteerModule.Application.Interfaces;
using ProjectPet.VolunteerModule.Domain.Models;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.PatchPet;

public class PatchPetHandler
{
    private readonly ISpeciesContract _speciesContract;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IReadDbContext _readDbContext;

    public PatchPetHandler(
        ISpeciesContract speciesContract,
        IVolunteerRepository volunteerRepository,
        IReadDbContext readDbContext)
    {
        _speciesContract = speciesContract;
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
            var speciesRes = await _speciesContract.GetSpeciesByIdAsync(command.AnimalData.SpeciesId, cancellationToken);
            if (speciesRes.IsFailure)
                return Error.Validation("record.not.found", $"Species with id \"{command.AnimalData.SpeciesId}\" was not found!");
            var species = speciesRes.Value;

            var breedRes = await _speciesContract.GetBreedByNameAsync(command.AnimalData.BreedName, cancellationToken);
            if (breedRes.IsFailure)
                return Error.Validation("record.not.found", $"Breed with name \"{command.AnimalData.BreedName}\" was not found!");
            var breed = breedRes.Value;

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
