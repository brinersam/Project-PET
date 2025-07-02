namespace ProjectPet.VolunteerModule.Contracts.Dto;
public record PetPhotoDto
(
    string FileId,
    string BucketName,
    string FileName,
    string ContentType,
    bool IsPrimary
)
{ }