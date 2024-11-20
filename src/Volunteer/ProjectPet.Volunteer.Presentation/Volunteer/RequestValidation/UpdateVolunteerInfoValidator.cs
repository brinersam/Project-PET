using FluentValidation;
using ProjectPet.API.Validation;
using ProjectPet.Application.UseCases.Volunteers.Commands.UpdateVolunteerInfo;
using ProjectPet.Core.Errors;
using ProjectPet.Core.Validator;
using ProjectPet.Domain.Models;
using ProjectPet.VolunteerModule.Presentation.Volunteer.Requests;

namespace ProjectPet.VolunteerModule.Presentation.Volunteer.RequestValidation;

public class UpdateVolunteerInfoValidator : AbstractValidator<UpdateVolunteerInfoCommand>
{
    public UpdateVolunteerInfoValidator()
    {
        RuleFor(u => u.Id)
            .NotNull()
            .WithError(Errors.General.ValueIsEmptyOrNull);
    }
}

public class UpdateVolunteerInfoRequestDtoValidator : AbstractValidator<UpdateVolunteerInfoRequest>
{
    public UpdateVolunteerInfoRequestDtoValidator()
    {
        RuleFor(u => u.dto.FullName)
            .MaximumLength(Constants.STRING_LEN_MEDIUM)
            .When(u => string.IsNullOrWhiteSpace(u.dto.FullName) == false);

        RuleFor(u => u.dto.Email)
            .MaximumLength(Constants.STRING_LEN_MEDIUM)
            .When(u => string.IsNullOrWhiteSpace(u.dto.Email) == false);

        RuleFor(u => u.dto.Description)
            .MaximumLength(Constants.STRING_LEN_MEDIUM)
            .When(u => string.IsNullOrWhiteSpace(u.dto.Description) == false);

        RuleFor(u => u.dto.Phonenumber)
            .ValidateValueObj(x => Phonenumber.Create
                                (x!.Phonenumber, x!.PhonenumberAreaCode))
            .When(u => u.dto.Phonenumber is not null);
    }
}
