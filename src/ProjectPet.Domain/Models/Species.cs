using ProjectPet.Domain.Models.DDD;

namespace ProjectPet.Domain.Models
{
    public class Species : Entity
    {
        public SpeciesID SpeciesId { get; private set; } = null!;
        public string Name { get; private set; } = null!;
        private List<Breed> _relatedBreeds = [];
        public IReadOnlyList<Breed> RelatedBreeds => _relatedBreeds;
        public Species(Guid id) : base(id) { } //efcore
        protected Species(Guid id, SpeciesID speciesId, string name, IEnumerable<Breed> breeds) : base(id)
        {
            SpeciesId = speciesId;
            Name = name;
            _relatedBreeds = breeds.ToList();
        }

        public static Species Create(Guid id, SpeciesID speciesId, string name, IEnumerable<Breed> breeds)
        {
            if (id.Equals(Guid.Empty))
                throw new ArgumentNullException("Argument id can not be empty!");

            if (speciesId.Equals(SpeciesID.Empty()))
                throw new ArgumentNullException("Argument speciesId can not be empty!");

            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("Argument name can not be empty!");

            return new Species(id, speciesId, name, breeds);
        }
    }
}
