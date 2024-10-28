namespace ProjectPet.Domain.Models;

public record SpeciesID
{
    public Guid Value { get; }

    private SpeciesID(Guid value)
    {
        Value = value;
    }

    public static SpeciesID New(Guid value) => new SpeciesID(value);
    public static SpeciesID New() => new SpeciesID(Guid.NewGuid());
    public static SpeciesID Empty() => new SpeciesID(Guid.Empty);
}