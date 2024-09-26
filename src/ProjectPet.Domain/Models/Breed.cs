using CSharpFunctionalExtensions;
using ProjectPet.Domain.Models.DDD;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Domain.Models
{
    public class Breed : EntityBase
    {
        public string Value { get; private set; } = null!;
        public Breed(Guid id) : base(id) { } //efcore
        private Breed(Guid id, string value) : base(id)
        {
            Value = value;
        }

        public static Result<Breed,Error> Create(Guid id, string value)
        {
            var strValidator = Validator.ValidatorString();

            if (id.Equals(Guid.Empty))
                return Errors.General.ValueIsEmptyOrNull(id,nameof(id));

            var result = strValidator.Check(value, nameof(value));
            if (result.IsFailure)
                return result.Error;

            return new Breed(id, value);
        }

    }
}
