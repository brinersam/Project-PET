using FluentValidation;
using ProjectPet.AccountsModule.Contracts.Requests;

namespace ProjectPet.AccountsModule.Presentation.RequestValidation.Account;

public class UpdateAccountPaymentRequestValidator : AbstractValidator<UpdateAccountPaymentRequest>
{
    public UpdateAccountPaymentRequestValidator()
    {
        //RuleForEach(c => c.PaymentInfos)
        //    .ValidateValueObj(x => PaymentInfo.Create(x.Title, x.Instructions)); // todo add validation
    }
}
