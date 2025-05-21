using FluentValidation;
using ProjectPet.Core.Validator;
using ProjectPet.SharedKernel;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerRequests.Contracts.Requests;
namespace ProjectPet.VolunteerRequests.Presentation.VolunteerRequests.RequestValidation;

public class UpdateValidator : AbstractValidator<UpdateVolunteerRequestRequest>
{
    public UpdateValidator()
    {
        RuleForEach(req => req.VolunteerAccountDto.PaymentInfos)
            .ChildRules(payInfo =>
            {
                payInfo.RuleFor(x => x.Instructions)
                    .NotEmpty()
                    .WithError(Errors.General.ValueIsEmptyOrNull)
                    .MaxLengthWithError(Constants.STRING_LEN_MEDIUM);

                payInfo.RuleFor(x => x.Title)
                    .NotEmpty()
                    .WithError(Errors.General.ValueIsEmptyOrNull)
                    .MaxLengthWithError(Constants.STRING_LEN_SMALL);
            });

        RuleForEach(req => req.VolunteerAccountDto.Certifications)
            .NotEmpty()
            .WithError(Errors.General.ValueIsEmptyOrNull)
            .MaxLengthWithError(Constants.STRING_LEN_MEDIUM);
    }
}