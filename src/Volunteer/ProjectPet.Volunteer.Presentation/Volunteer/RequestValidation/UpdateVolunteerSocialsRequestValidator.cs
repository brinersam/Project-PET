using FluentValidation;
using ProjectPet.Core.Validator;
using ProjectPet.VolunteerModule.Contracts.Requests;
using ProjectPet.VolunteerModule.Domain.Models;

namespace ProjectPet.VolunteerModule.Presentation.Volunteer.RequestValidation;

public class UpdateVolunteerSocialsRequestValidator : AbstractValidator<UpdateVolunteerSocialsRequest>
{
    public UpdateVolunteerSocialsRequestValidator()
    {
        RuleForEach(c => c.SocialNetworks)
            .ValidateValueObj(x => SocialNetwork.Create(x.Name, x.Link));
    }

}
