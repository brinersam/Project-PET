namespace ProjectPet.Application.Dto;
public class PetDto
{
    public Guid Id { get; set; }
    public Guid ModuleId { get; set; }
    public string Name { get; set; } = null!;
}
