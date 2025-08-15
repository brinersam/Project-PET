using ProjectPet.FileService.Contracts.Dtos;

namespace ProjectPet.VolunteerModule.Contracts.Events;
public record PetDeletedEvent(
    Guid PetId,
    List<FileLocationDto> FileLocations)
{ }