using ProjectPet.Core.Requests;
using ProjectPet.VolunteerModule.Contracts.Dto;
using ProjectPet.VolunteerModule.Contracts.Requests;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.FinishPetPhotoUpload;
public record FinishPetPhotoUploadCommand(
    List<FinishPetPhotoUploadDto> Files,
    Guid VolunteerId,
    Guid PetId) : IMapFromRequest<FinishPetPhotoUploadCommand, FinishPetPhotoUploadRequest, Guid, Guid>
{
    public static FinishPetPhotoUploadCommand FromRequest(FinishPetPhotoUploadRequest req, Guid volunteerId, Guid petId)
    {
        return new FinishPetPhotoUploadCommand(
                   req.FileUploadDto.ToList(),
                   volunteerId,
                   petId);
    }
}
