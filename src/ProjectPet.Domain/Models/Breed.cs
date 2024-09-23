using ProjectPet.Domain.Models.DDD;

namespace ProjectPet.Domain.Models
{
    public class Breed : Entity
    {
        public string Value { get; private set; } = null!;
        public Breed(Guid id) : base(id)
        {
        }
    }
}
