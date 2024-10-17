using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProjectPet.API.Response;
using ProjectPet.Domain.Shared;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;

namespace ProjectPet.API.Validation
{
    public class CustomResultFactory : IFluentValidationAutoValidationResultFactory
    {
        public IActionResult CreateActionResult(
            ActionExecutingContext context,
            ValidationProblemDetails? validationProblemDetails)
        {
            if (validationProblemDetails is null)
                throw new NullReferenceException(nameof(validationProblemDetails));

            List<FieldError> errors = [];

            foreach (var (invalidField, validaitonErrors) in validationProblemDetails.Errors)
            {
                List<FieldError> responseErrors = [];
                foreach (var error in validaitonErrors)
                {
                    FieldError fieldError = null!;

                    if (Error.TryDeserialize(error, out Error deserialized))
                    {
                        fieldError = new FieldError(
                            deserialized.Code,deserialized.Message,invalidField);
                    }
                    else
                        fieldError = new FieldError(null, error, invalidField);

                    responseErrors.Add(fieldError);
                }

                errors.AddRange(responseErrors);
            }

            var envelope = Envelope.Error(errors);
            return new ObjectResult(envelope) { StatusCode = StatusCodes.Status400BadRequest };
        }
    }
}
