using CSharpFunctionalExtensions;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Domain.Models
{
    public record AnimalData
    {
        public SpeciesID SpeciesID { get; } = null!;
        public Guid BreedID { get; }
        private AnimalData(SpeciesID speciesID, Guid breedID)
        {
            SpeciesID = speciesID;
            BreedID = breedID;
        }
        public static Result<AnimalData,Error> Create(SpeciesID speciesID, Guid breedID)
        {
            var validator = Validator.ValidatorNull<Guid>();

            var result = validator.Check(breedID, nameof(breedID));
            if (result.IsFailure)
                return result.Error;

            result = validator.Check(speciesID.Value, nameof(speciesID));
            if (result.IsFailure)
                return result.Error;

            return new AnimalData(speciesID, breedID);
        }

    }
}