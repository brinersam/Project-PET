using ProjectPet.Domain.Models.DDD;

namespace ProjectPet.Domain.Models
{
    public class Species : Entity
    {
        public new SpeciesID Id { get; private set; } = null!;
        private List<Breed> _relatedBreeds = [];
        public IReadOnlyList<Breed> RelatedBreeds => _relatedBreeds;

        public Species(Guid id) : base(id)
        {
        }
    }
}
