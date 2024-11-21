using FluentValidation;
using ProjectPet.API.Validation;
using ProjectPet.VolunteerModule.Domain.Models;
using ProjectPet.VolunteerModule.Presentation.Volunteer.Requests;

namespace ProjectPet.VolunteerModule.Presentation.Volunteer.RequestValidation;

public class UpdateVolunteerSocialsRequestValidator : AbstractValidator<UpdateVolunteerSocialsRequest>
{
    public UpdateVolunteerSocialsRequestValidator()
    {
        RuleForEach(c => c.SocialNetworks)
            .ValidateValueObj(x => SocialNetwork.Create(x.Name, x.Link));
    }

}
