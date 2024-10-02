using FluentValidation;
using ProjectPet.Application.Validation;
using ProjectPet.Domain.Models;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Application.UseCases.CreateVolunteer
{
    public class CreateVolunteerRequestValidator : AbstractValidator<CreateVolunteerRequestDto>
    {
        public CreateVolunteerRequestValidator()
        {
            RuleFor(c => c.Phonenumber)
                .ValidateValueObj(x => PhoneNumber.Create
                                    (x.Phonenumber,x.PhonenumberAreaCode));

            RuleFor(c => c.FullName)
                .NotNull()
                .WithMessage(Errors.General.ValueIsEmptyOrNull("{PropertyValue}", "{PropertyName}").Message)
                .MaximumLength(Constants.STRING_LEN_MEDIUM)
                .WithMessage(Errors.General
                    .ValueLengthMoreThan("{PropertyValue}", "{PropertyName}", "{MaxLength}").Message);

            RuleFor(c => c.Email)
                .NotNull()
                .WithMessage(Errors.General.ValueIsEmptyOrNull("{PropertyValue}", "{PropertyName}").Message)
                .MaximumLength(Constants.STRING_LEN_MEDIUM)
                .WithMessage(Errors.General
                    .ValueLengthMoreThan("{PropertyValue}", "{PropertyName}", "{MaxLength}").Message);

            RuleFor(c => c.Description)
                .NotNull()
                .WithMessage(Errors.General.ValueIsEmptyOrNull("{PropertyValue}", "{PropertyName}").Message)
                .MaximumLength(Constants.STRING_LEN_MEDIUM)
                .WithMessage(Errors.General
                    .ValueLengthMoreThan("{PropertyValue}", "{PropertyName}", "{MaxLength}").Message);

            RuleForEach(c => c.PaymentMethods)
                .ValidateValueObj(x => PaymentInfo.Create(x.Title, x.Instructions));

            RuleForEach(c => c.SocialNetworks)
                .ValidateValueObj(x => SocialNetwork.Create(x.Name, x.Link));
        }
    }

}
