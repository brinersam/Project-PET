using FluentValidation;
using ProjectPet.Core.Validator;
using ProjectPet.VolunteerModule.Contracts.Requests;
using ProjectPet.VolunteerModule.Domain.Models;

namespace ProjectPet.VolunteerModule.Presentation.Volunteer.RequestValidation;

public class UpdateVolunteerPaymentRequestValidator : AbstractValidator<UpdateVolunteerPaymentRequest>
{
    public UpdateVolunteerPaymentRequestValidator()
    {
        RuleForEach(c => c.PaymentInfos)
            .ValidateValueObj(x => PaymentInfo.Create(x.Title, x.Instructions));
    }
}
