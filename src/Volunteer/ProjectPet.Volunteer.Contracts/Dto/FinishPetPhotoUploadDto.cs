using ProjectPet.FileService.Contracts.Dtos;

namespace ProjectPet.VolunteerModule.Contracts.Dto;
public record FinishPetPhotoUploadDto(FinishFileUploadDto uploadData, string FileName, string ContentType)
{ }
