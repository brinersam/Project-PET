using FluentValidation;
using ProjectPet.Core.Validator;
using ProjectPet.SharedKernel;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerRequests.Contracts.Requests;

namespace ProjectPet.VolunteerRequests.Presentation.VolunteerRequests.RequestValidation;

public class RejectValidator : AbstractValidator<RejectVolunteerRequestRequest>
{
    public RejectValidator()
    {
        RuleFor(req => req.RejectionComment)
            .NotEmpty()
            .WithError(Errors.General.ValueIsEmptyOrNull)
            .MaxLengthWithError(Constants.STRING_LEN_MEDIUM);
    }
}