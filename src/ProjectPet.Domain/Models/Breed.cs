using ProjectPet.Domain.Models.DDD;

namespace ProjectPet.Domain.Models
{
    public class Breed : EntityBase
    {
        public string Value { get; private set; } = null!;
        public Breed(Guid id) : base(id) { } //efcore
        protected Breed(Guid id, string value) : base(id)
        {
            Value = value;
        }

        public static Breed Create(Guid id, string value)
        {
            if (id.Equals(Guid.Empty))
                throw new ArgumentNullException("Argument id can not be empty!");

            if (String.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("Argument value can not be empty!");

            return new Breed(id, value);
        }

    }
}
