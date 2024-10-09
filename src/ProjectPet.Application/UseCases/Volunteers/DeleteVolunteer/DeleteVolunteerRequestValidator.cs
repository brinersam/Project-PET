using FluentValidation;

namespace ProjectPet.Application.UseCases.Volunteers
{
    public class DeleteVolunteerRequestValidator : AbstractValidator<DeleteVolunteerRequest>
    {
        public DeleteVolunteerRequestValidator()
        {
            RuleFor(d => d.Id).NotEmpty();
        }
    }
}
