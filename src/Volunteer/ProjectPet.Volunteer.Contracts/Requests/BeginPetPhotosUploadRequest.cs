using ProjectPet.FileService.Contracts.Dtos;

namespace ProjectPet.VolunteerModule.Contracts.Requests;

public record BeginPetPhotosUploadRequest(List<BeginFileUploadDto> FileUploadRequests);
