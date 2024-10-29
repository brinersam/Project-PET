using CSharpFunctionalExtensions;
using ProjectPet.Domain.Models.DDD;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Domain.Models;

public class Breed : EntityBase
{
    public string Value { get; private set; } = null!;
    public Breed(Guid id) : base(id) { } //efcore
    private Breed(Guid id, string value) : base(id)
    {
        Value = value;
    }

    public static Result<Breed, Error> Create(string value)
    {
        //if we use Guid.NewGuide, then on attempt at creating through one to many relationship we get
        //The database operation was expected to affect 1 row(s), but actually affected 0 row(s)
        var id = Guid.Empty;
        var strValidator = Validator.ValidatorString();

        var result = strValidator.Check(value, nameof(value));
        if (result.IsFailure)
            return result.Error;

        return new Breed(id, value);
    }

}
