using ProjectPet.API.Etc;
using ProjectPet.Application.Dto;
using ProjectPet.Application.UseCases.Volunteers.Commands.UploadPetPhoto;

namespace ProjectPet.API.Requests.Shared;

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


