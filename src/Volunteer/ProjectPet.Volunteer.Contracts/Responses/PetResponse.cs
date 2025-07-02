using ProjectPet.Core.Requests;
using ProjectPet.VolunteerModule.Contracts.Dto;

namespace ProjectPet.VolunteerModule.Contracts.Responses;
public record PetResponse(
    Guid Id,
    Guid VolunteerId,
    Guid SpeciesID,
    Guid BreedID,
    string Name,
    string Description,
    string Coat,
    DateOnly DateOfBirth,
    string Status,
    List<PetPhotoResponse> Photos) : IMapFromRequest<PetResponse, PetDto, IEnumerable<PetPhotoResponse>> // todo rename interface to IMapFrom
{
    public static PetResponse FromRequest(PetDto dto, IEnumerable<PetPhotoResponse> photos) 
    {
        return new PetResponse(
            dto.Id,
            dto.VolunteerId,
            dto.SpeciesID,
            dto.BreedID,
            dto.Name,
            dto.Description,
            dto.Coat,
            dto.DateOfBirth,
            dto.Status,
            photos.ToList());
    }
}

public record PetPhotoResponse(string FileName,
                               string FileId,
                               string Url)
{ }