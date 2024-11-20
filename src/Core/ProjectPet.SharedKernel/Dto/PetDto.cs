namespace ProjectPet.Core.Dto;
public class PetDto
{
    public Guid Id { get; set; }
    public Guid VolunteerId { get; set; }
    public Guid SpeciesID { get; set; }
    public Guid BreedID { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Coat { get; set; } = null!;
    public DateOnly DateOfBirth { get; set; }
    public string Status { get; set; } = null!;
}
