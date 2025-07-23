using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.VolunteerModule.Contracts.Responses;
public record DeletePetPhotosResponse(
    Error? Error,
    string FileId)
{ }
