using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProjectPet.Framework;
using ProjectPet.Framework.Authorization;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.Approve;
using ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.Create;
using ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.Reject;
using ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.RequestRevision;
using ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.Review;
using ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.Update;
using ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Queries.GetByAdminIdPaginatedFiltered;
using ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Queries.GetByUserIdPaginatedFiltered;
using ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Queries.GetUnassignedPaginated;
using ProjectPet.VolunteerRequests.Contracts.Requests;

namespace ProjectPet.VolunteerRequests.Presentation.VolunteerRequests;
public class VolunteerRequestsController : CustomControllerBase
{
    [Permission(PermissionCodes.VolunteerRequestCreate)]
    [HttpPost()]
    public async Task<IActionResult> CreateVolunteerRequest(
        [FromServices] CreateHandler handler,
        IValidator<CreateVolunteerRequestRequest> validator,
        [FromBody] CreateVolunteerRequestRequest request,
        CancellationToken cancellationToken = default)
    {
        var validatorRes = await validator.ValidateAsync(request, cancellationToken);
        if (validatorRes.IsValid == false)
            return Envelope.ToResponse(validatorRes.Errors);

        var userIdRes = GetCurrentUserId();
        if (userIdRes.IsFailure)
            return userIdRes.Error.ToResponse();

        var command = CreateCommand.FromRequest(request, userIdRes.Value);
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [Permission(PermissionCodes.VolunteerRequestUpdate)]
    [HttpPut("{requestId:Guid}")]
    public async Task<IActionResult> UpdateVolunteerRequest(
        [FromServices] UpdateHandler handler,
        [FromBody] UpdateVolunteerRequestRequest request,
        IValidator<UpdateVolunteerRequestRequest> validator,
        [FromRoute] Guid requestId,
        CancellationToken cancellationToken = default)
    {
        var validatorRes = await validator.ValidateAsync(request, cancellationToken);
        if (validatorRes.IsValid == false)
            return Envelope.ToResponse(validatorRes.Errors);

        var command = UpdateCommand.FromRequest(request, requestId);
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [Permission(PermissionCodes.VolunteerRequestAdmin)]
    [HttpPut("{requestId:Guid}/review")]
    public async Task<IActionResult> ReviewVolunteerRequest(
        [FromServices] ReviewHandler handler,
        [FromRoute] Guid requestId,
        CancellationToken cancellationToken = default)
    {
        var adminIdRes = GetCurrentUserId();
        if (adminIdRes.IsFailure)
            return adminIdRes.Error.ToResponse();

        var command = new ReviewCommand(requestId, adminIdRes.Value);
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [Permission(PermissionCodes.VolunteerRequestAdmin)]
    [HttpPut("{requestId:Guid}/request-revision")]
    public async Task<IActionResult> RequestRevisionVolunteerRequest(
        [FromServices] RequestRevisionHandler handler,
        [FromBody] RequestRevisionVolunteerRequestRequest request,
        IValidator<RequestRevisionVolunteerRequestRequest> validator,
        [FromRoute] Guid requestId,
        CancellationToken cancellationToken = default)
    {
        var validatorRes = await validator.ValidateAsync(request, cancellationToken);
        if (validatorRes.IsValid == false)
            return Envelope.ToResponse(validatorRes.Errors);

        var command = RequestRevisionCommand.FromRequest(request, requestId);
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [Permission(PermissionCodes.VolunteerRequestAdmin)]
    [HttpPut("{requestId:Guid}/reject")]
    public async Task<IActionResult> RejectVolunteerRequest(
        [FromServices] RejectHandler handler,
        [FromBody] RejectVolunteerRequestRequest request,
        [FromRoute] Guid requestId,
        IValidator<RejectVolunteerRequestRequest> validator,
        CancellationToken cancellationToken = default)
    {
        var validatorRes = await validator.ValidateAsync(request, cancellationToken);
        if (validatorRes.IsValid == false)
            return Envelope.ToResponse(validatorRes.Errors);

        var command = RejectCommand.FromRequest(request, requestId, PermissionCodes.VolunteerRequestCreate);
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [Permission(PermissionCodes.VolunteerRequestAdmin)]
    [HttpPut("{requestId:Guid}/approve")]
    public async Task<IActionResult> ApproveVolunteerRequest(
        [FromServices] ApproveHandler handler,
        [FromRoute] Guid requestId,
        CancellationToken cancellationToken = default)
    {
        var command = new ApproveCommand(requestId);
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [Permission(PermissionCodes.VolunteerRequestAdmin)]
    [HttpGet("unassigned")]
    public async Task<IActionResult> GetUnassignedVolunteerRequestsPaginated(
        [FromServices] GetUnassignedPaginatedHandler handler,
        [FromQuery] GetVolunteerRequestsPaginatedRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = GetUnassignedPaginatedQuery.FromRequest(request);
        var result = await handler.HandleAsync(query, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [Permission(PermissionCodes.VolunteerRequestAdmin)]
    [HttpGet("byadmin/{adminId:Guid?}")]
    public async Task<IActionResult> GetVolunteerRequestsByAdminId(
        [FromServices] GetByAdminIdPaginatedFilteredHandler handler,
        [FromQuery] GetVolunteerRequestFilteredPaginatedRequest request,
        [FromQuery] Guid? adminId,
        CancellationToken cancellationToken = default)
    {
        if (adminId is null || Equals(adminId, Guid.Empty))
        {
            var getAdminIdRes = GetCurrentUserId();
            if (getAdminIdRes.IsFailure)
                return getAdminIdRes.Error.ToResponse();
            adminId = getAdminIdRes.Value;
        }

        var query = GetByAdminIdPaginatedFilteredQuery.FromRequest(request, (Guid)adminId);
        var result = await handler.HandleAsync(query, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [Permission(PermissionCodes.VolunteerRequestRead)]
    [HttpGet("byuser")]
    public async Task<IActionResult> GetVolunteerRequestsByUserId(
        [FromServices] GetByUserIdPaginatedFilteredHandler handler,
        [FromQuery] GetVolunteerRequestFilteredPaginatedRequest request,
        CancellationToken cancellationToken = default)
    {
        var getUserIdRes = GetCurrentUserId();
        if (getUserIdRes.IsFailure)
            return getUserIdRes.Error.ToResponse();

        var query = GetByUserIdPaginatedFilteredQuery.FromRequest(request, getUserIdRes.Value);
        var result = await handler.HandleAsync(query, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    private Result<Guid, Error> GetCurrentUserId()
    {
        string? userId = HttpContext.User.Claims.FirstOrDefault(u => u.Properties.Values.Contains("sub"))?.Value;
        if (String.IsNullOrWhiteSpace(userId))
            return Error.Failure("claim.not.found", "Unknown user!");
        return new Guid(userId);
    }
}
