using FluentValidation;
using ProjectPet.Application.Validation;
using ProjectPet.Domain.Models;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Application.UseCases.Volunteers
{
    public class UpdateVolunteerInfoRequestValidator : AbstractValidator<UpdateVolunteerInfoRequest>
    {
        public UpdateVolunteerInfoRequestValidator()
        {
            RuleFor(u => u.Id).NotNull(); //.WithMessage(); // todo
        }
    }

    public class UpdateVolunteerInfoRequestDtoValidator : AbstractValidator<UpdateVolunteerInfoRequestDto>
    {
        public UpdateVolunteerInfoRequestDtoValidator()
        {
            RuleFor(u => u.FullName)
                .MaximumLength(Constants.STRING_LEN_MEDIUM)
                .When(u => String.IsNullOrWhiteSpace(u.FullName) == false);

            RuleFor(u => u.Email)
                .MaximumLength(Constants.STRING_LEN_MEDIUM)
                .When(u => String.IsNullOrWhiteSpace(u.Email) == false);

            RuleFor(u => u.Description)
                .MaximumLength(Constants.STRING_LEN_MEDIUM)
                .When(u => String.IsNullOrWhiteSpace(u.Description) == false);

            RuleFor(u => u.PhoneNumber)
                .ValidateValueObj(x => PhoneNumber.Create
                                    (x.Phonenumber, x.PhonenumberAreaCode))
                .When(u => u.PhoneNumber is not null);
        }
    }
}
