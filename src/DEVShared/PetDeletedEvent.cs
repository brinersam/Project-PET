using ProjectPet.FileService.Contracts.Dtos;

namespace DEVShared;
public record PetDeletedEvent(
    Guid petId,
    List<FileLocationDto> fileLocations) //todo carry this to NEW contracts just for the originating slice
{ }