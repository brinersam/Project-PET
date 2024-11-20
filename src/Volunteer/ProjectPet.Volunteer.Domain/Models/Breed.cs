using CSharpFunctionalExtensions;
using ProjectPet.Core.Entities.AbstractBase;
using ProjectPet.Domain.Shared;

namespace ProjectPet.VolunteerModule.Domain.Models;

public class Breed : EntityBase
{
    public Guid SpeciesId { get; private set; }
    public string Value { get; private set; } = null!;
    public Breed(Guid id) : base(id) { } //efcore
    private Breed(Guid id, Guid speciesId, string value) : base(id)
    {
        SpeciesId = speciesId;
        Value = value;
    }

    public static Result<Breed, Error> Create(string value)
    {
        //if we use Guid.NewGuide, then on attempt at creating through one to many relationship we get
        //The database operation was expected to affect 1 row(s), but actually affected 0 row(s)
        var id = Guid.Empty;
        var speciesId = Guid.Empty;
        var strValidator = Validator.ValidatorString();

        var result = strValidator.Check(value, nameof(value));
        if (result.IsFailure)
            return result.Error;

        return new Breed(id, speciesId, value);
    }

}
