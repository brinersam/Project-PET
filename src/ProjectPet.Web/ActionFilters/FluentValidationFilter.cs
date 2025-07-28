using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProjectPet.Framework;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.Web.ActionFilters;

public class FluentValidationFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            List<Error> errors = [];

            foreach (var item in context.ModelState)
            {
                if (item.Value.Errors.Count <= 0)
                    continue;

                foreach (var error in item.Value.Errors)
                {
                    errors.Add(Error.Validation("value.failed.validation", $"Value [{item.Key}]: {error.ErrorMessage}"));
                }
            }

            var responseObj = EnvelopeErrors<Error>.Create(errors);

            context.Result = new JsonResult(responseObj)
            {
                StatusCode = 400,
            };
        }
    }
}