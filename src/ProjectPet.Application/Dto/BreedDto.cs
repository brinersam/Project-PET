namespace ProjectPet.Application.Dto;
public class BreedDto
{
    public Guid Id { get; set; }
    public Guid SpeciesId { get; set; }
    public string Value { get; private set; } = null!;
}
