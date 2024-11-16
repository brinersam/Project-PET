namespace ProjectPet.Application.Dto;
public class PetDto
{
    public Guid Id { get; set; }
    public Guid volunteer_id { get; set; }
    public Guid AnimalDataSpeciesID { get; set; }
    public Guid AnimalDataBreedID { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Status { get; set; } = null!;
}
