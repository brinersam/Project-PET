using ProjectPet.Application.Dto;

namespace ProjectPet.Application.UseCases.Volunteers.UploadPetPhoto;

public record UploadPetPhotoRequest(
    Guid VolunteerId,
    Guid PetId,
    string Title,
    List<FileDto> Files);