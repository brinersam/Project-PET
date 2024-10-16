using CSharpFunctionalExtensions;
using FluentValidation;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Application.Validation
{
    public static class CustomValidators
    {
        public static IRuleBuilderOptionsConditions<T, TBuilder> ValidateValueObj<T, TBuilder, TValueObject>(
            this IRuleBuilder<T, TBuilder> ruleBuilder,
            Func<TBuilder, Result<TValueObject, Error>> factoryMethod)
        {
            return ruleBuilder.Custom((value, context) =>
            {
                Result<TValueObject, Error> result = factoryMethod(value);

                if (result.IsSuccess)
                    return;

                context.AddFailure(result.Error.Message);
            });
        }

    }

    public static class ErrorShortcuts
    {
        public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> ruleBuilder,
            Func<TProperty?, string, Error> error)
        {
            return ruleBuilder
                .WithMessage((x, y) =>
                    $"{error.Invoke(y, "{PropertyName}").Message}");
        }

        public static IRuleBuilderOptions<T, string> MaxLengthWithError<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> ruleBuilder,
            int maxLen)
        {
            return ((IRuleBuilderOptions <T, string>)ruleBuilder)
                .MaximumLength(maxLen)
                .WithMessage((x, y) => 
                    $"{Errors.General.ValueLengthMoreThan(y, "{PropertyName}", maxLen).Message}");
        }
    }
}
