namespace ProjectPet.SpeciesModule.Contracts.Dto;
public class SpeciesDto
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Guid Id { get; init; }
    public string Name { get; init; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
