using ProjectPet.FileService.Contracts.Dtos;

namespace DEVShared;
public record PetDeletedEvent(
    Guid PetId,
    List<FileLocationDto> FileLocations) //todo carry this to NEW contracts just for the originating slice
{ }