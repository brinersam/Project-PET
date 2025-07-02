using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.VolunteerModule.Contracts.Dto;

public record BeginPetPhotoUploadDto(Error? Error, string FileName, string FileId, string UploadId, string[] Urls)
{ }
