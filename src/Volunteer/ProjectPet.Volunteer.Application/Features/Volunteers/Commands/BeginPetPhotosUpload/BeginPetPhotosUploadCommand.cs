using ProjectPet.Core.Requests;
using ProjectPet.FileService.Contracts.Dtos;
using ProjectPet.VolunteerModule.Contracts.Requests;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.BeginPetPhotosUpload;

public record BeginPetPhotosUploadCommand(
    Guid VolunteerId,
    Guid PetId,
    List<BeginFileUploadDto> FileUploadDtos) : IMapFromRequest<BeginPetPhotosUploadCommand, BeginPetPhotosUploadRequest, Guid, Guid>
{
    public static BeginPetPhotosUploadCommand FromRequest(BeginPetPhotosUploadRequest req, Guid volunteerId, Guid petId)
    {
        return new BeginPetPhotosUploadCommand(
                   volunteerId,
                   petId,
                   req.FileUploadRequests.ToList());
    }
}
