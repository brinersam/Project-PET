using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectPet.AccountsModule.Application.Features.Account.Commands.UpdateAccountPayment;
using ProjectPet.AccountsModule.Application.Features.Account.Commands.UpdateAccountSocials;
using ProjectPet.AccountsModule.Application.Features.Account.Queries;
using ProjectPet.AccountsModule.Contracts.Requests;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.Framework;
using ProjectPet.Framework.Authorization;

namespace ProjectPet.AccountsModule.Presentation;
public class AccountController : CustomControllerBase
{
    [Authorize]
    [Permission(PermissionCodes.SelfVolunteerEdit)]
    [HttpPut("payment")]
    public async Task<IActionResult> PatchPayment(
        [FromServices] UserScopedData userData,
        [FromServices] UpdateAccountPaymentHandler handler,
        IValidator<UpdateAccountPaymentRequest> validator,
        [FromBody] UpdateAccountPaymentRequest request,
        CancellationToken cancellationToken = default)
    {
        var validatorRes = await validator.ValidateAsync(request, cancellationToken);
        if (validatorRes.IsValid == false)
            return Envelope.ToResponse(validatorRes.Errors);

        if (userData.IsSuccess == false)
            return userData.Error!.ToResponse();

        var cmd = UpdateAccountPaymentCommand.FromRequest(request, (Guid)userData.UserId!);

        var result = await handler.HandleAsync(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [Permission(PermissionCodes.SelfMemberEdit)]
    [HttpPut("social")]
    public async Task<ActionResult<Guid>> PatchSocial(
        [FromServices] UserScopedData userData,
        [FromServices] UpdateAccountSocialsHandler handler,
        IValidator<UpdateAccountSocialsRequest> validator,
        [FromBody] UpdateAccountSocialsRequest request,
        CancellationToken cancellationToken = default)
    {
        var validatorRes = await validator.ValidateAsync(request, cancellationToken);
        if (validatorRes.IsValid == false)
            return Envelope.ToResponse(validatorRes.Errors);

        if (userData.IsSuccess == false)
            return userData.Error!.ToResponse();

        var cmd = UpdateAccountSocialsCommand.FromRequest(request, (Guid)userData.UserId!);

        var result = await handler.HandleAsync(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [Permission(PermissionCodes.AdminMasterkey)]
    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<Guid>> GetUserInfo(
        [FromServices] GetUserInfoHandler handler,
        [FromRoute] Guid userId,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.HandleAsync(userId, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}
