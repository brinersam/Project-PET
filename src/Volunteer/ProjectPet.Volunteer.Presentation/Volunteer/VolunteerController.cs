using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProjectPet.Core.Files;
using ProjectPet.Framework;
using ProjectPet.Framework.Authorization;
using ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.CreatePet;
using ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.CreateVolunteer;
using ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.DeletePet;
using ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.DeletePetPhotos;
using ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.DeleteVolunteer;
using ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.FinishPetPhotoUpload;
using ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.PatchPet;
using ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.SetMainPetPhoto;
using ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.UpdatePetStatus;
using ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.UpdateVolunteerInfo;
using ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.UploadPetPhoto;
using ProjectPet.VolunteerModule.Application.Features.Volunteers.Queries.GetVolunteerById;
using ProjectPet.VolunteerModule.Application.Features.Volunteers.Queries.GetVolunteers;
using ProjectPet.VolunteerModule.Contracts.Requests;

namespace ProjectPet.VolunteerModule.Presentation.Volunteer;

[Permission(PermissionCodes.VolunteerCreate)]
public class VolunteerController : CustomControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Post(
        [FromServices] CreateVolunteerHandler handler,
        IValidator<CreateVolunteerRequest> validator,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var validatorRes = await validator.ValidateAsync(request, cancellationToken);
        if (validatorRes.IsValid == false)
            return Envelope.ToResponse(validatorRes.Errors);

        var cmd = CreateVolunteerCommand.FromRequest(request);
        var result = await handler.HandleAsync(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("{id:guid}/Pet")]
    public async Task<ActionResult<Guid>> PostPet(
        [FromRoute] Guid id,
        [FromServices] CreatePetHandler handler,
        [FromBody] CreatePetRequest request,
        IValidator<CreatePetRequest> validator,
        CancellationToken cancellationToken = default)
    {
        var validatorRes = await validator.ValidateAsync(request, cancellationToken);
        if (validatorRes.IsValid == false)
            return Envelope.ToResponse(validatorRes.Errors);

        var cmd = CreatePetCommand.FromRequest(request, id);

        var result = await handler.HandleAsync(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("{volunteerId:guid}/pet/{petid:guid}/photos/start")]
    public async Task<IActionResult> BeginPetPhotosUpload(
        [FromServices] BeginPetPhotosUploadHandler handler,
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petid,
        [FromBody] BeginPetPhotosUploadRequest request,
        IValidator<BeginPetPhotosUploadRequest> validator,
        CancellationToken cancellationToken = default)
    {
        var validatorRes = await validator.ValidateAsync(request, cancellationToken);
        if (validatorRes.IsValid == false)
            return Envelope.ToResponse(validatorRes.Errors);

        var cmd = BeginPetPhotosUploadCommand.FromRequest(request, volunteerId, petid);

        var result = await handler.HandleAsync(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> Delete(
        [FromServices] DeleteVolunteerHandler handler,
        [FromBody] DeleteVolunteerRequest request,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var cmd = DeleteVolunteerCommand.FromRequest(request);

        var result = await handler.HandleAsync(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPatch("{id:guid}/main")]
    public async Task<ActionResult<Guid>> PatchVolunteer(
        [FromServices] UpdateVolunteerInfoHandler handler,
        [FromBody] UpdateVolunteerInfoRequest request,
        [FromRoute] Guid id,
        IValidator<UpdateVolunteerInfoRequest> validator,
        CancellationToken cancellationToken = default)
    {
        var validatorRes = await validator.ValidateAsync(request, cancellationToken);
        if (validatorRes.IsValid == false)
            return Envelope.ToResponse(validatorRes.Errors);

        var cmd = UpdateVolunteerInfoCommand.FromRequest(request, id);

        var result = await handler.HandleAsync(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetVolunteersPaginated(
        [FromServices] GetVolunteerPaginatedHandler handler,
        [FromQuery] GetVolunteerPaginatedRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = GetVolunteerPaginatedQuery.FromRequest(request);
        var result = await handler.HandleAsync(query, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetVolunteerById(
        [FromServices] GetVolunteerByIdHandler handler,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetVolunteerByIdQuery(id);
        var result = await handler.HandleAsync(query, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{volunteerId:guid}/pet/{petid:guid}/status")]
    public async Task<IActionResult> UpdatePetStatus(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petid,
        [FromServices] UpdatePetStatusHandler handler,
        [FromBody] UpdatePetStatusRequest request,
        CancellationToken cancellationToken = default)
    {
        var cmd = new UpdatePetStatusCommand(volunteerId, petid, request.Status);
        var result = await handler.HandleAsync(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [HttpDelete("{volunteerId:guid}/pet/{petid:guid}/photos")]
    public async Task<IActionResult> DeletePetPhotos(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petid,
        [FromServices] DeletePetPhotosHandler handler,
        [FromBody] DeletePetPhotosRequest request,
        CancellationToken cancellationToken = default)
    {
        var cmd = new DeletePetPhotosCommand(volunteerId, petid, request.photoPathsToDelete);
        var result = await handler.HandleAsync(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [HttpPatch("{volunteerId:guid}/pet/{petid:guid}")]
    public async Task<IActionResult> PatchPet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petid,
        [FromServices] PatchPetHandler handler,
        [FromBody] PatchPetRequest request,
        IValidator<PatchPetRequest> validator,
        CancellationToken cancellationToken = default)
    {
        var validatorRes = await validator.ValidateAsync(request, cancellationToken);
        if (validatorRes.IsValid == false)
            return Envelope.ToResponse(validatorRes.Errors);

        var cmd = PatchPetCommand.FromRequest(request, volunteerId, petid);
        var result = await handler.HandleAsync(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [HttpDelete("{volunteerId:guid}/pet/{petid:guid}")]
    public async Task<IActionResult> DeletePet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petid,
        [FromQuery] bool softDelete,
        [FromServices] DeletePetHandler handler,
        CancellationToken cancellationToken = default)
    {
        var cmd = new DeletePetCommand(volunteerId, petid, softDelete);
        var result = await handler.HandleAsync(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [HttpPost("{volunteerId:guid}/pet/{petid:guid}/photos/main")]
    public async Task<IActionResult> SetMainPetPhoto(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petid,
        [FromBody] SetMainPetPhotoRequest request,
        [FromServices] SetMainPetPhotoHandler handler,
        CancellationToken cancellationToken = default)
    {
        var cmd = new SetMainPetPhotoCommand(volunteerId, petid, request.PhotoPath);
        var result = await handler.HandleAsync(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }
}
