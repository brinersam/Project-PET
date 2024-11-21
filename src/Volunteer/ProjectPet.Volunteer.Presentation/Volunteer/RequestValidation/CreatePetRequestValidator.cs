using FluentValidation;
using ProjectPet.API.Validation;
using ProjectPet.VolunteerModule.Domain.Models;
using ProjectPet.VolunteerModule.Presentation.Volunteer.Requests;

namespace ProjectPet.VolunteerModule.Presentation.Volunteer.RequestValidation;

public class CreatePetRequestValidator : AbstractValidator<CreatePetRequest>
{
    public CreatePetRequestValidator()
    {
        RuleFor(p => p.AnimalData)
            .ValidateValueObj(a => AnimalData.Create(a.SpeciesId, Guid.NewGuid()));

        RuleForEach(p => p.PaymentInfos)
            .ValidateValueObj(p => PaymentInfo.Create(p.Title, p.Instructions));

        RuleFor(p => p.Phonenumber)
            .ValidateValueObj(x => Phonenumber.Create(
                x.Phonenumber,
                x.PhonenumberAreaCode));

        RuleFor(p => p.Address)
            .ValidateValueObj(x => Address.Create(
                x.Name,
                x.Street,
                x.Building,
                x.Block,
                x.Entrance,
                x.Floor,
                x.Apartment));

        RuleFor(p => p.HealthInfo)
            .ValidateValueObj(x => HealthInfo.Create(
                x.Health,
                x.IsSterilized,
                x.IsVaccinated,
                x.Weight,
                x.Height));
    }
}
