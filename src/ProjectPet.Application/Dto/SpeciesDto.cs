namespace ProjectPet.Application.Dto;
public class SpeciesReadDto
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Guid Id { get; init; }
    public string Name { get; init; }
    public BreedReadDto[] RelatedBreeds { get; init; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
