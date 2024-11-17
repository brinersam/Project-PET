using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProjectPet.API.Extentions;
using ProjectPet.API.Processors;
using ProjectPet.API.Requests.Shared;
using ProjectPet.API.Requests.Volunteers;
using ProjectPet.API.Response;
using ProjectPet.Application.Dto;
using ProjectPet.Application.UseCases.Volunteers.Commands.CreatePet;
using ProjectPet.Application.UseCases.Volunteers.Commands.CreateVolunteer;
using ProjectPet.Application.UseCases.Volunteers.Commands.DeletePet;
using ProjectPet.Application.UseCases.Volunteers.Commands.DeletePetPhotos;
using ProjectPet.Application.UseCases.Volunteers.Commands.DeleteVolunteer;
using ProjectPet.Application.UseCases.Volunteers.Commands.PatchPet;
using ProjectPet.Application.UseCases.Volunteers.Commands.UpdatePetStatus;
using ProjectPet.Application.UseCases.Volunteers.Commands.UpdateVolunteerInfo;
using ProjectPet.Application.UseCases.Volunteers.Commands.UpdateVolunteerPayment;
using ProjectPet.Application.UseCases.Volunteers.Commands.UpdateVolunteerSocials;
using ProjectPet.Application.UseCases.Volunteers.Commands.UploadPetPhoto;
using ProjectPet.Application.UseCases.Volunteers.Queries.GetVolunteerById;
using ProjectPet.Application.UseCases.Volunteers.Queries.GetVolunteers;

namespace ProjectPet.API.Controllers;

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

        var cmd = request.ToCommand();
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

        var cmd = request.ToCommand(id);

        var result = await handler.HandleAsync(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("{volunteerId:guid}/pet/{petid:guid}/photos")]
    public async Task<IActionResult> UploadPetPhoto(
        [FromServices] UploadPetPhotoHandler service,
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petid,
        [FromForm] UploadFileRequest req,
        CancellationToken cancellationToken = default)
    {
        await using var processor = new FormFileProcessor();

        List<FileDto> filesDto = processor.Process(req.Files);

        var cmd = req.ToCommand(volunteerId, petid, filesDto);

        var result = await service.HandleAsync(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> Delete(
        [FromServices] DeleteVolunteerHandler handler,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var request = new DeleteVolunteerCommand(id);

        var result = await handler.HandleAsync(request, cancellationToken);

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

        var cmd = new UpdateVolunteerInfoCommand(id, request.dto);

        var result = await handler.HandleAsync(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/payment")]
    public async Task<ActionResult<Guid>> PatchPayment(
        [FromServices] UpdateVolunteerPaymentHandler handler,
        [FromRoute] Guid id,
        IValidator<UpdateVolunteerPaymentRequest> validator,
        [FromBody] UpdateVolunteerPaymentRequest request,
        CancellationToken cancellationToken = default)
    {
        var validatorRes = await validator.ValidateAsync(request, cancellationToken);
        if (validatorRes.IsValid == false)
            return Envelope.ToResponse(validatorRes.Errors);

        var cmd = new UpdateVolunteerPaymentCommand(id, request.PaymentInfos);

        var result = await handler.HandleAsync(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }


    [HttpPut("{id:guid}/social")]
    public async Task<ActionResult<Guid>> PatchSocial(
        [FromServices] UpdateVolunteerSocialsHandler handler,
        [FromRoute] Guid id,
        IValidator<UpdateVolunteerSocialsRequest> validator,
        [FromBody] UpdateVolunteerSocialsRequest request,
        CancellationToken cancellationToken = default)
    {
        var validatorRes = await validator.ValidateAsync(request, cancellationToken);
        if (validatorRes.IsValid == false)
            return Envelope.ToResponse(validatorRes.Errors);

        var cmd = new UpdateVolunteerSocialsCommand(id, request.SocialNetworks);

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
        var query = request.ToCommand();
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

        var cmd = request.ToCommand(volunteerId, petid);
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
}
