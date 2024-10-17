using FluentValidation;
using ProjectPet.Application.Validation;
using ProjectPet.Domain.Models;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Application.UseCases.Volunteers
{
    public class CreateVolunteerRequestValidator : AbstractValidator<CreateVolunteerRequestDto>
    {
        public CreateVolunteerRequestValidator()
        {
            RuleFor(c => c.Phonenumber)
                .NotNull()
                .ValidateValueObj(x => PhoneNumber.Create
                                    (x.Phonenumber, x.PhonenumberAreaCode));

            RuleFor(c => c.FullName)
                .NotEmpty()
                .WithError(Errors.General.ValueIsEmptyOrNull)
                .MaxLengthWithError(Constants.STRING_LEN_MEDIUM);
                
            RuleFor(c => c.Email)
                .NotEmpty()
                .WithError(Errors.General.ValueIsEmptyOrNull)
                .MaxLengthWithError(Constants.STRING_LEN_MEDIUM);

            RuleFor(c => c.Description)
                .NotEmpty()
                .WithError(Errors.General.ValueIsEmptyOrNull)
                .MaxLengthWithError(Constants.STRING_LEN_MEDIUM);

            RuleForEach(c => c.PaymentMethods)
                .ValidateValueObj(x => PaymentInfo.Create(x.Title, x.Instructions));

            RuleForEach(c => c.SocialNetworks)
                .ValidateValueObj(x => SocialNetwork.Create(x.Name, x.Link));
        }
    }

}
