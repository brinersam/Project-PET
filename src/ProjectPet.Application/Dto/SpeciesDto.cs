namespace ProjectPet.Application.Dto;
public class SpeciesReadDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public List<string> RelatedBreeds { get; init; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public SpeciesReadDto() { } //efcore
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public SpeciesReadDto(
        Guid id,
        string name,
        List<string> relatedBreeds)
    {
        Id = id;
        Name = name;
        RelatedBreeds = relatedBreeds;
    }
}
