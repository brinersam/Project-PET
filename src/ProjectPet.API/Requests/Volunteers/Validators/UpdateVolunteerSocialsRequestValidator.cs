using FluentValidation;
using ProjectPet.API.Validation;
using ProjectPet.Domain.Models;

namespace ProjectPet.API.Requests.Volunteers.Validators;

public class UpdateVolunteerSocialsRequestValidator : AbstractValidator<UpdateVolunteerSocialsRequest>
{
    public UpdateVolunteerSocialsRequestValidator()
    {
        RuleForEach(c => c.SocialNetworks)
            .ValidateValueObj(x => SocialNetwork.Create(x.Name, x.Link));
    }

}
