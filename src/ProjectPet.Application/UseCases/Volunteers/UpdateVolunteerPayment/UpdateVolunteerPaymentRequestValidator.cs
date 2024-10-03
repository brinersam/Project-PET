using FluentValidation;
using ProjectPet.Application.Validation;
using ProjectPet.Domain.Models;

namespace ProjectPet.Application.UseCases.Volunteers
{
    public class UpdateVolunteerPaymentRequestValidator : AbstractValidator<UpdateVolunteerPaymentRequestDto>
    {
        public UpdateVolunteerPaymentRequestValidator()
        {
            RuleForEach(c => c.PaymentInfos)
                .ValidateValueObj(x => PaymentInfo.Create(x.Title, x.Instructions));
        }
    }
}
