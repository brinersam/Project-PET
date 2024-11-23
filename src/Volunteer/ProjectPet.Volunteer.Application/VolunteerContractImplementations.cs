using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerModule.Application.Interfaces;
using ProjectPet.VolunteerModule.Contracts;
using ProjectPet.VolunteerModule.Contracts.Dto;

namespace ProjectPet.VolunteerModule.Application;
public class VolunteerContractImplementations : IVolunteerContract
{
    private readonly IReadDbContext _readDbContext;

    public VolunteerContractImplementations(
        IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Result<PetDto, Error>> GetPetByBreedIdAsync(
        Guid breedId,
        CancellationToken cancellationToken = default)
    {
        var pet = await _readDbContext.Pets.FirstOrDefaultAsync(
            x => x.BreedID == breedId,
            cancellationToken);

        if (pet == null)
            return Errors.General.NotFound(typeof(PetDto));

        return pet;
    }


    public async Task<Result<PetDto, Error>> GetPetBySpeciesIdAsync(
        Guid speciesId,
        CancellationToken cancellationToken = default)
    {
        var pet = await _readDbContext.Pets.FirstOrDefaultAsync(
            x => x.SpeciesID == speciesId,
            cancellationToken);

        if (pet == null)
            return Errors.General.NotFound(typeof(PetDto));

        return pet;
    }
}
