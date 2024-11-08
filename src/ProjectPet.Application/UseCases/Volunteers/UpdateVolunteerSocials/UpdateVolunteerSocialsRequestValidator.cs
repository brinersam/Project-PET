using FluentValidation;
using ProjectPet.Application.Extensions;
using ProjectPet.Domain.Models;

namespace ProjectPet.Application.UseCases.Volunteers.UpdateVolunteerSocials;

public class UpdateVolunteerSocialsRequestValidator : AbstractValidator<UpdateVolunteerSocialsRequestDto>
{
    public UpdateVolunteerSocialsRequestValidator()
    {
        RuleForEach(c => c.SocialNetworks)
            .ValidateValueObj(x => SocialNetwork.Create(x.Name, x.Link));
    }

}
