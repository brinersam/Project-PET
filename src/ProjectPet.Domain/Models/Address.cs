using CSharpFunctionalExtensions;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Domain.Models;

public record Address
{
    public string Name { get; } = null!;
    public string Street { get; } = null!;
    public string Building { get; } = null!;
    public string? Block { get; }
    public int? Entrance { get; }
    public int? Floor { get; }
    public int Apartment { get; }

    private Address(
        string name,
        string street,
        string building,
        string? block,
        int? entrance,
        int? floor,
        int apartment)
    {
        Name = name;
        Street = street;
        Building = building;
        Block = block;
        Entrance = entrance;
        Floor = floor;
        Apartment = apartment;
    }

    public static Result<Address, Error> Create(
        string name,
        string street,
        string building,
        string? block,
        int? entrance,
        int? floor,
        int apartment)
    {

        var strValidator = Validator.ValidatorString();

        var result = strValidator.Check(name, nameof(name));
        if (result.IsFailure)
            return result.Error;

        result = strValidator.Check(street, nameof(street));
        if (result.IsFailure)
            return result.Error;

        result = strValidator.Check(building, nameof(building));
        if (result.IsFailure)
            return result.Error;

        return new Address
        (
            name,
            street,
            building,
            block,
            entrance,
            floor,
            apartment
        );
    }

}