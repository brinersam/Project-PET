using FluentValidation;
using ProjectPet.Core.Validator;
using ProjectPet.DiscussionsModule.Contracts.Requests;
using ProjectPet.SharedKernel;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.DiscussionsModule.Presentation.Discussions.RequestValidation;
public class AddMessageToDiscussionValidation : AbstractValidator<AddMessageToDiscussionRequest>
{
    public AddMessageToDiscussionValidation()
    {
        RuleFor(req => req.MessageBody)
            .NotEmpty()
            .WithError(Errors.General.ValueIsEmptyOrNull)
            .MaxLengthWithError(Constants.STRING_LEN_SMALL);
    }
}