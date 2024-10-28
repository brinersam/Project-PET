using FluentValidation;
using ProjectPet.Application.Validation;
using ProjectPet.Domain.Models;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Application.UseCases.Volunteers.UpdateVolunteerInfo;

public class UpdateVolunteerInfoRequestValidator : AbstractValidator<UpdateVolunteerInfoRequest>
{
    public UpdateVolunteerInfoRequestValidator()
    {
        RuleFor(u => u.Id)
            .NotNull()
            .WithError(Errors.General.ValueIsEmptyOrNull);
    }
}

public class UpdateVolunteerInfoRequestDtoValidator : AbstractValidator<UpdateVolunteerInfoRequestDto>
{
    public UpdateVolunteerInfoRequestDtoValidator()
    {
        RuleFor(u => u.FullName)
            .MaximumLength(Constants.STRING_LEN_MEDIUM)
            .When(u => string.IsNullOrWhiteSpace(u.FullName) == false);

        RuleFor(u => u.Email)
            .MaximumLength(Constants.STRING_LEN_MEDIUM)
            .When(u => string.IsNullOrWhiteSpace(u.Email) == false);

        RuleFor(u => u.Description)
            .MaximumLength(Constants.STRING_LEN_MEDIUM)
            .When(u => string.IsNullOrWhiteSpace(u.Description) == false);

        RuleFor(u => u.PhoneNumber)
            .ValidateValueObj(x => PhoneNumber.Create
                                (x.Phonenumber, x.PhonenumberAreaCode))
            .When(u => u.PhoneNumber is not null);
    }
}
