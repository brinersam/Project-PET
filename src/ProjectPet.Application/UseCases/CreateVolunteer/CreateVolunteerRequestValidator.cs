using FluentValidation;
using ProjectPet.Application.Validation;
using ProjectPet.Domain.Models;

namespace ProjectPet.Application.UseCases.CreateVolunteer
{
    public class CreateVolunteerRequestValidator : AbstractValidator<CreateVolunteerRequestDto>
    {
        public CreateVolunteerRequestValidator()
        {
            RuleFor(c => new{
                c.Phonenumber,
                c.PhonenumberAreaCode
            }).ValidateValueObj(x => 
                PhoneNumber.Create(x.Phonenumber,x.PhonenumberAreaCode));

            RuleForEach(c => c.PaymentMethods)
                .ValidateValueObj(x => PaymentInfo.Create(x.Title, x.Instructions));

            RuleForEach(c => c.SocialNetworks)
                .ValidateValueObj(x => SocialNetwork.Create(x.Name, x.Link));
        }
    }

}
