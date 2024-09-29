using CSharpFunctionalExtensions;
using FluentValidation;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Application.Validation
{
    public static class CustomValidators
    {
        public static IRuleBuilderOptionsConditions<T, TBuilder> ValidateValueObj<T, TBuilder, TValueObject>(
            this IRuleBuilder<T, TBuilder> ruleBuilder,
            Func<TBuilder, Result<TValueObject,Error>> factoryMethod)
        {
            return ruleBuilder.Custom((value, context) =>
            {
                Result<TValueObject,Error> result = factoryMethod(value);

                if (result.IsSuccess)
                    return;

                context.AddFailure(result.Error.Message);
            });
        }
        
    }
}
