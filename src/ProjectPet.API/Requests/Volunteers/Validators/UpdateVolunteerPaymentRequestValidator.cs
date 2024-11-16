using FluentValidation;
using ProjectPet.Application.Extensions;
using ProjectPet.Domain.Models;

namespace ProjectPet.API.Requests.Volunteers.Validators;

public class UpdateVolunteerPaymentRequestValidator : AbstractValidator<UpdateVolunteerPaymentRequest>
{
    public UpdateVolunteerPaymentRequestValidator()
    {
        RuleForEach(c => c.PaymentInfos)
            .ValidateValueObj(x => PaymentInfo.Create(x.Title, x.Instructions));
    }
}
