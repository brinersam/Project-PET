using CSharpFunctionalExtensions;
using ProjectPet.Domain.Models.DDD;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Domain.Models
{
    public class Species : EntityBase
    {
        public SpeciesID SpeciesId { get; private set; } = null!;
        public string Name { get; private set; } = null!;
        private List<Breed> _relatedBreeds;
        public IReadOnlyList<Breed> RelatedBreeds => _relatedBreeds;
        public Species(Guid id) : base(id) { } //efcore
        private Species(Guid id, SpeciesID speciesId, string name, IEnumerable<Breed> breeds) : base(id)
        {
            SpeciesId = speciesId;
            Name = name;
            _relatedBreeds = breeds.ToList();
        }

        public static Result<Species,Error> Create(Guid id, SpeciesID speciesId, string name, IEnumerable<Breed> breeds)
        {
            var validatorID = Validator.ValidatorNull<Guid>();

            var resultID = validatorID.Check(id, nameof(id));
            if (resultID.IsFailure)
                return resultID.Error;

            resultID = validatorID.Check(speciesId.Value, nameof(speciesId));
            if (resultID.IsFailure)
                return resultID.Error;

            var result = Validator
                .ValidatorString()
                .Check(name, nameof(name));

            if (result.IsFailure)
                return result.Error;

            return new Species(id, speciesId, name, breeds);
        }
    }
}
