using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectPet.AccountsModule.Application.Features.Account.Commands.UpdateAccountPayment;
using ProjectPet.AccountsModule.Application.Features.Account.Commands.UpdateAccountSocials;
using ProjectPet.AccountsModule.Contracts.Requests;
using ProjectPet.Framework;
using ProjectPet.SharedKernel.ErrorClasses;
using System.IdentityModel.Tokens.Jwt;

namespace ProjectPet.AccountsModule.Presentation;
[Authorize]
public class AccountController : CustomControllerBase
{
    [HttpPut("payment")]
    public async Task<IActionResult> PatchPayment(
        [FromServices] UpdateAccountPaymentHandler handler,
        IValidator<UpdateAccountPaymentRequest> validator,
        [FromBody] UpdateAccountPaymentRequest request,
        CancellationToken cancellationToken = default)
    {
        var validatorRes = await validator.ValidateAsync(request, cancellationToken);
        if (validatorRes.IsValid == false)
            return Envelope.ToResponse(validatorRes.Errors);

        string? userId = HttpContext.User.Claims.FirstOrDefault(u => u.Properties.Values.Contains("sub"))?.Value;
        if (String.IsNullOrWhiteSpace(userId))
            return Error.Failure("claim.not.found", "Unknown user!").ToResponse();

        var cmd = UpdateAccountPaymentCommand.FromRequest(request, new Guid(userId));

        var result = await handler.HandleAsync(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }


    [HttpPut("social")]
    public async Task<ActionResult<Guid>> PatchSocial(
        [FromServices] UpdateAccountSocialsHandler handler,
        IValidator<UpdateAccountSocialsRequest> validator,
        [FromBody] UpdateAccountSocialsRequest request,
        CancellationToken cancellationToken = default)
    {
        var validatorRes = await validator.ValidateAsync(request, cancellationToken);
        if (validatorRes.IsValid == false)
            return Envelope.ToResponse(validatorRes.Errors);

        string? userId = HttpContext.User.Claims.FirstOrDefault(u => u.Properties.Values.Contains("sub"))?.Value;
        if (String.IsNullOrWhiteSpace(userId))
            return Error.Failure("claim.not.found", "Unknown user!").ToResponse();

        var cmd = UpdateAccountSocialsCommand.FromRequest(request, new Guid(userId));

        var result = await handler.HandleAsync(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }
}
