using ProjectPet.SharedKernel.Dto;
using ProjectPet.Application.UseCases.Volunteers.Commands.UploadPetPhoto;
using ProjectPet.Core.Abstractions;

namespace ProjectPet.VolunteerModule.Presentation.Volunteer.Requests;

public record UploadFileRequest(
    string Title,
    IFormFileCollection Files) : IToCommand<UploadPetPhotoCommand, Guid, Guid, IEnumerable<FileDto>>
{
    public UploadPetPhotoCommand ToCommand(Guid volunteerId, Guid petId, IEnumerable<FileDto> filesDto)
    {
        return new UploadPetPhotoCommand(
                   volunteerId,
                   petId,
                   Title,
                   filesDto.ToList());
    }
}


