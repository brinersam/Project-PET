using FluentValidation;
using ProjectPet.API.Validation;
using ProjectPet.VolunteerModule.Domain.Models;
using ProjectPet.VolunteerModule.Presentation.Volunteer.Requests;

namespace ProjectPet.VolunteerModule.Presentation.Volunteer.RequestValidation;

public class UpdateVolunteerPaymentRequestValidator : AbstractValidator<UpdateVolunteerPaymentRequest>
{
    public UpdateVolunteerPaymentRequestValidator()
    {
        RuleForEach(c => c.PaymentInfos)
            .ValidateValueObj(x => PaymentInfo.Create(x.Title, x.Instructions));
    }
}
