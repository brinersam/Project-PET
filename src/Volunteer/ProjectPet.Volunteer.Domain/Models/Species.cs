using CSharpFunctionalExtensions;
using ProjectPet.Core.Entities.AbstractBase;
using ProjectPet.Domain.Shared;

namespace ProjectPet.VolunteerModule.Domain.Models;

public class Species : EntityBase
{
    public string Name { get; private set; } = null!;
    private List<Breed> _relatedBreeds = null!;
    public IReadOnlyList<Breed> RelatedBreeds => _relatedBreeds;
    public Species(Guid id) : base(id) { } //efcore
    private Species(Guid id, string name) : base(id)
    {
        Name = name;
    }

    public static Result<Species, Error> Create(Guid Id, string name)
    {
        var validatorID = Validator.ValidatorNull<Guid>();

        var resultID = validatorID.Check(Id, nameof(Id));
        if (resultID.IsFailure)
            return resultID.Error;

        var result = Validator
            .ValidatorString()
            .Check(name, nameof(name));

        if (result.IsFailure)
            return result.Error;

        return new Species(Id, name);
    }

    public UnitResult<Error> AddNewBreed(Breed breed)
    {
        if (TryFindBreedByName(breed.Value, out _))
            return Error.Validation("value.not.unique",
                    $"Can not add a duplicate breed {breed.Value} to species {Name}!");

        _relatedBreeds.Add(breed);
        return Result.Success<Error>();
    }

    public UnitResult<Error> RemoveBreed(Guid breedId)
    {
        int idx = _relatedBreeds.FindIndex(x => x.Id == breedId);

        if (idx != -1)
            _relatedBreeds.RemoveAt(idx);

        return Result.Success<Error>();
    }

    public bool TryFindBreedByName(string name, out Breed result)
    {
        result = RelatedBreeds.FirstOrDefault(x => x.Value == name)!;
        if (result is null)
            return false;

        return true;
    }
}
