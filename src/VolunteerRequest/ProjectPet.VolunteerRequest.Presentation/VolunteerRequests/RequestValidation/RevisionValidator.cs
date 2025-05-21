using FluentValidation;
using ProjectPet.Core.Validator;
using ProjectPet.SharedKernel;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerRequests.Contracts.Requests;

namespace ProjectPet.VolunteerRequests.Presentation.VolunteerRequests.RequestValidation;
public class RequestRevisionValidator : AbstractValidator<RequestRevisionVolunteerRequestRequest>
{
    public RequestRevisionValidator()
    {
        RuleFor(req => req.RevisionComment)
            .NotEmpty()
            .WithError(Errors.General.ValueIsEmptyOrNull)
            .MaxLengthWithError(Constants.STRING_LEN_MEDIUM);
    }
}