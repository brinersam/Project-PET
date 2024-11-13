using FluentValidation;
using ProjectPet.API.Validation;
using ProjectPet.Domain.Models;
using ProjectPet.Domain.Shared;

namespace ProjectPet.API.Requests.Volunteers.Validators;

public class CreateVolunteerRequestValidator : AbstractValidator<CreateVolunteerRequest>
{
    public CreateVolunteerRequestValidator()
    {
        RuleFor(c => c.VolunteerDto.Phonenumber)
            .NotNull()
            .ValidateValueObj(x => Phonenumber.Create
                                (x.Phonenumber, x.PhonenumberAreaCode));

        RuleFor(c => c.VolunteerDto.FullName)
            .NotEmpty()
            .WithError(Errors.General.ValueIsEmptyOrNull)
            .MaxLengthWithError(Constants.STRING_LEN_MEDIUM);

        RuleFor(c => c.VolunteerDto.Email)
            .NotEmpty()
            .WithError(Errors.General.ValueIsEmptyOrNull)
            .MaxLengthWithError(Constants.STRING_LEN_MEDIUM);

        RuleFor(c => c.VolunteerDto.Description)
            .NotEmpty()
            .WithError(Errors.General.ValueIsEmptyOrNull)
            .MaxLengthWithError(Constants.STRING_LEN_MEDIUM);

        RuleForEach(c => c.PaymentInfoDtos)
            .ValidateValueObj(x => PaymentInfo.Create(x.Title, x.Instructions));

        RuleForEach(c => c.SocialNetworkDtos)
            .ValidateValueObj(x => SocialNetwork.Create(x.Name, x.Link));
    }
}
