using ProjectPet.Core.Abstractions;
using ProjectPet.Core.Providers;
using ProjectPet.VolunteerModule.Contracts.Requests;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.UploadPetPhoto;

public record UploadPetPhotoCommand(
    Guid VolunteerId,
    Guid PetId,
    string Title,
    List<FileDto> Files) : IMapFromRequest<UploadPetPhotoCommand, UploadFileRequest, Guid, Guid, IEnumerable<FileDto>>
{
    public static UploadPetPhotoCommand FromRequest(UploadFileRequest req, Guid volunteerId, Guid petId, IEnumerable<FileDto> filesDto)
    {
        return new UploadPetPhotoCommand(
                   volunteerId,
                   petId,
                   req.Title,
                   filesDto.ToList());
    }
}
