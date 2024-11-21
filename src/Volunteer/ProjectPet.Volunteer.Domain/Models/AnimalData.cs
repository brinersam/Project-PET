using CSharpFunctionalExtensions;
using ProjectPet.Core.Validator;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.VolunteerModule.Domain.Models;

public record AnimalData
{
    public Guid SpeciesID { get; }
    public Guid BreedID { get; }
    private AnimalData(Guid speciesID, Guid breedID)
    {
        SpeciesID = speciesID;
        BreedID = breedID;
    }
    public static Result<AnimalData, Error> Create(Guid speciesID, Guid breedID)
    {
        var validator = Validator.ValidatorNull<Guid>();

        var result = validator.Check(breedID, nameof(breedID));
        if (result.IsFailure)
            return result.Error;

        result = validator.Check(speciesID, nameof(speciesID));
        if (result.IsFailure)
            return result.Error;

        return new AnimalData(speciesID, breedID);
    }

}