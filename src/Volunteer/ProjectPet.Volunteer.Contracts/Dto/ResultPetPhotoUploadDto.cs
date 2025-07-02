using ProjectPet.FileService.Contracts.Dtos;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.VolunteerModule.Contracts.Dto;
public record ResultPetPhotoUploadDto(Error? Error, PetPhotoUploadData uploadData)
{ }

public record PetPhotoUploadData(FileLocationDto Location, string FileName, string ContentType)
{ }