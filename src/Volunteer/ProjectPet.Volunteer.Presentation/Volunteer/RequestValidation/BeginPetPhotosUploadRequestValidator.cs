using FluentValidation;
using ProjectPet.Core.Validator;
using ProjectPet.VolunteerModule.Contracts.Requests;
using ProjectPet.VolunteerModule.Domain.Models;

namespace ProjectPet.VolunteerModule.Presentation.Volunteer.RequestValidation;
public class BeginPetPhotosUploadRequestValidator : AbstractValidator<BeginPetPhotosUploadRequest>
{
    public BeginPetPhotosUploadRequestValidator()
    {
        RuleForEach(req => req.FileUploadRequests)
            .ValidateValueObj(x => PetPhoto.Create(
                                "futureFileId",
                                x.BucketName,
                                x.FileName,
                                x.ContentType)
            );
    }
}
