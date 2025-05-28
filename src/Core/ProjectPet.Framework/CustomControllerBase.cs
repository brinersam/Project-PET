using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.Framework;

[ApiController]
[Route("api/[controller]")]
public abstract class CustomControllerBase : ControllerBase
{
    public BadRequestObjectResult BadRequest<T>([ActionResultObjectValue] T[] error)
    {
        var envelope = Envelope.Error([error]);
        return new BadRequestObjectResult(envelope);
    }
    public override BadRequestObjectResult BadRequest([ActionResultObjectValue] object? error)
        => BadRequest([error]);

    public override OkObjectResult Ok([ActionResultObjectValue] object? value)
    {
        var envelope = Envelope.Ok(value);
        return new OkObjectResult(envelope);
    }

    protected Result<Guid, Error> GetCurrentUserId()
    {
        string? userId = HttpContext.User.Claims.FirstOrDefault(u => u.Properties.Values.Contains("sub"))?.Value;
        if (String.IsNullOrWhiteSpace(userId))
            return Error.Failure("claim.not.found", "Unknown user!");
        return new Guid(userId);
    }
}
