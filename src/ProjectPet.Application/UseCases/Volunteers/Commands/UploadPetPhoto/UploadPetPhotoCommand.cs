using ProjectPet.Application.Dto;

namespace ProjectPet.Application.UseCases.Volunteers.Commands.UploadPetPhoto;

public record UploadPetPhotoCommand(
    Guid VolunteerId,
    Guid PetId,
    string Title,
    List<FileDto> Files);