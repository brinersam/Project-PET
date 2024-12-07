using FluentValidation;
using ProjectPet.AccountsModule.Contracts.Requests;
using ProjectPet.Core.Validator;

namespace ProjectPet.AccountsModule.Presentation.RequestValidation.Account;

public class UpdateAccountSocialsRequestValidator : AbstractValidator<UpdateAccountSocialsRequest>
{
    public UpdateAccountSocialsRequestValidator()
    {
        //RuleForEach(c => c.SocialNetworks)
        //    .ValidateValueObj(x => SocialNetwork.Create(x.Name, x.Link)); // todo add validation
    }

}
