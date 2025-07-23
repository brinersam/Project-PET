using ProjectPet.VolunteerModule.Contracts.Dto;

namespace ProjectPet.VolunteerModule.Contracts.Requests;
public record FinishPetPhotoUploadRequest(List<FinishPetPhotoUploadDto> FileUploadDto)
{ }
