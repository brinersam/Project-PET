using FluentValidation;
using ProjectPet.API.Validation;
using ProjectPet.Domain.Models;
using ProjectPet.VolunteerModule.Presentation.Volunteer.Requests;

namespace ProjectPet.VolunteerModule.Presentation.Volunteer.RequestValidation;

public class PatchPetRequestValidator : AbstractValidator<PatchPetRequest>
{
    public PatchPetRequestValidator()
    {
        RuleFor(p => p.AnimalData)
            .ValidateValueObj(a => AnimalData.Create(a.SpeciesId, Guid.NewGuid()))
            .When(p => p.AnimalData is not null);

        RuleFor(p => p.HealthInfo)
            .ValidateValueObj(x => HealthInfo.Create(
                x.Health,
                x.IsSterilized,
                x.IsVaccinated,
                x.Weight,
                x.Height))
            .When(p => p.HealthInfo is not null);

        RuleFor(p => p.Address)
            .ValidateValueObj(x => Address.Create(
                x.Name,
                x.Street,
                x.Building,
                x.Block,
                x.Entrance,
                x.Floor,
                x.Apartment))
            .When(p => p.Address is not null);

        RuleFor(p => p.Phonenumber)
            .ValidateValueObj(x => Phonenumber.Create(
                x.Phonenumber,
                x.PhonenumberAreaCode))
            .When(p => p.Phonenumber is not null);
    }
}
