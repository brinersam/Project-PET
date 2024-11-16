using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;
using ProjectPet.Domain.Shared;

namespace ProjectPet.API.Validation;

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

            ValidationFailure failure = new()
            {
                ErrorMessage = result.Error.Message,
                PropertyName = context.PropertyName,
                ErrorCode = result.Error.Code
            };

            context.AddFailure(failure);
        });
    }

    // todo maybe refactor to decorate out of box validators
    // with our error class into custom validator (like above)
    #region CustomErrors

    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> ruleBuilder,
        Func<TProperty?, string, Error> errorFactory)
    {
        ruleBuilder.WithErrorCode(errorFactory.Invoke(default, string.Empty).Code); // ugly
        return ruleBuilder
            .WithMessage((x, y) =>
                $"{errorFactory.Invoke(y, "{PropertyName}").Message}");
    }

    public static IRuleBuilderOptions<T, string> MaxLengthWithError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> ruleBuilder,
        int maxLen)
    {
        return ((IRuleBuilderOptions<T, string>)ruleBuilder)
            .MaximumLength(maxLen)
            .WithErrorCode(Errors.General.ValueLengthMoreThan(0, string.Empty, 0).Code) // ugly
            .WithMessage((x, y) =>
                $"{Errors.General.ValueLengthMoreThan(y, "{PropertyName}", maxLen).Message}");
    }

    #endregion
}
