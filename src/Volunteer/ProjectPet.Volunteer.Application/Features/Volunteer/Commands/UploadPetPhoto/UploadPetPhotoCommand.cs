using ProjectPet.SharedKernel.Dto;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteer.Commands.UploadPetPhoto;

public record UploadPetPhotoCommand(
    Guid VolunteerId,
    Guid PetId,
    string Title,
    List<FileDto> Files);