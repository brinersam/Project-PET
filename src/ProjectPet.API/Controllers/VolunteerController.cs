using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProjectPet.API.Extentions;
using ProjectPet.API.Processors;
using ProjectPet.API.Requests.Shared;
using ProjectPet.API.Requests.Volunteers;
using ProjectPet.API.Response;
using ProjectPet.Application.Dto;
using ProjectPet.Application.Repositories;
using ProjectPet.Application.UseCases.Volunteers.Commands.CreatePet;
using ProjectPet.Application.UseCases.Volunteers.Commands.CreateVolunteer;
using ProjectPet.Application.UseCases.Volunteers.Commands.DeleteVolunteer;
using ProjectPet.Application.UseCases.Volunteers.Commands.UpdateVolunteerInfo;
using ProjectPet.Application.UseCases.Volunteers.Commands.UpdateVolunteerPayment;
using ProjectPet.Application.UseCases.Volunteers.Commands.UpdateVolunteerSocials;
using ProjectPet.Application.UseCases.Volunteers.Commands.UploadPetPhoto;

namespace ProjectPet.API.Controllers;

public class VolunteerController : CustomControllerBase
{
    private readonly IVolunteerRepository _volunteerRepository;

    public VolunteerController(IVolunteerRepository volunteerRepository)
    {
        _volunteerRepository = volunteerRepository;
    }

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
        CancellationToken cancellationToken = default)
    {
        var cmd = request.ToCommand(id);

        var result = await handler.HandleAsync(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("{id:guid}/Pet/{petid:guid}/Photos")]
    public async Task<IActionResult> UploadPetPhoto(
        [FromServices] UploadPetPhotoHandler service,
        [FromRoute] Guid id,
        [FromRoute] Guid petid,
        [FromForm] UploadFileRequest req,
        CancellationToken cancellationToken = default)
    {
        await using (var processor = new FormFileProcessor())
        {
            List<FileDto> filesDto = processor.Process(req.Files);

            var cmd = req.ToCommand(id, petid, filesDto);

            var result = await service.HandleAsync(cmd, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> Delete(
        [FromServices] DeleteVolunteerHandler handler,
        [FromRoute] Guid id,
        IValidator<DeleteVolunteerCommand> validator,
        CancellationToken cancellationToken = default)
    {
        var request = new DeleteVolunteerCommand(id);

        var validatorRes = await validator.ValidateAsync(request, cancellationToken);
        if (validatorRes.IsValid == false)
            return BadRequest(validatorRes.Errors);

        var result = await handler.HandleAsync(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPatch("{id:guid}/main")]
    public async Task<ActionResult<Guid>> PatchInfo(
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
}
