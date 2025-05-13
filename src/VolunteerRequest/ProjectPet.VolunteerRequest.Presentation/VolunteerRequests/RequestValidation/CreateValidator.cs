using FluentValidation;
using ProjectPet.Core.Validator;
using ProjectPet.SharedKernel;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerRequests.Contracts.Requests;

namespace ProjectPet.VolunteerRequests.Presentation.VolunteerRequests.RequestValidation;
public class CreateValidator : AbstractValidator<CreateVolunteerRequestRequest>
{
    public CreateValidator()
    {
        RuleForEach(req => req.AccountDto.PaymentInfos)
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

        RuleForEach(req => req.AccountDto.Certifications)
            .NotEmpty()
            .WithError(Errors.General.ValueIsEmptyOrNull)
            .MaxLengthWithError(Constants.STRING_LEN_MEDIUM);
    }
}

