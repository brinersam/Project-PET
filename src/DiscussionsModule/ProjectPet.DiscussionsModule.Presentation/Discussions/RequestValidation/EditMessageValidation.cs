using FluentValidation;
using ProjectPet.Core.Validator;
using ProjectPet.DiscussionsModule.Contracts.Requests;
using ProjectPet.SharedKernel;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.DiscussionsModule.Presentation.Discussions.RequestValidation;
public class EditMessageValidation : AbstractValidator<EditMessageRequest>
{
    public EditMessageValidation()
    {
        RuleFor(req => req.MessageBody)
            .NotEmpty()
            .WithError(Errors.General.ValueIsEmptyOrNull)
            .MaxLengthWithError(Constants.STRING_LEN_SMALL);
    }
}