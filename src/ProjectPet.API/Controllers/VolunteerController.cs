using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProjectPet.API.Contracts.FileManagement;
using ProjectPet.API.Extentions;
using ProjectPet.API.Processors;
using ProjectPet.Application.Dto;
using ProjectPet.Application.UseCases.Volunteers;
using ProjectPet.Application.UseCases.Volunteers.CreatePet;
using ProjectPet.Application.UseCases.Volunteers.CreateVolunteer;
using ProjectPet.Application.UseCases.Volunteers.DeleteVolunteer;
using ProjectPet.Application.UseCases.Volunteers.UpdateVolunteerInfo;
using ProjectPet.Application.UseCases.Volunteers.UpdateVolunteerPayment;
using ProjectPet.Application.UseCases.Volunteers.UpdateVolunteerSocials;
using ProjectPet.Application.UseCases.Volunteers.UploadPetPhoto;

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
        [FromBody] CreateVolunteerRequestDto dto,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.HandleAsync(dto, cancellationToken);

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
        var cmd = new CreatePetCommand(
            id,
            request.Name,
            request.Coat,
            request.Description,
            DateOnly.FromDateTime(request.DateOfBirth),
            request.AnimalData_SpeciesId,
            request.AnimalData_BreedName,
            request.HealthInfo,
            request.PaymentInfos,
            request.Address,
            request.PhoneNumber,
            request.Status);

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
        [FromForm] UploadFileDto dto,
        CancellationToken cancellationToken = default)
    {
        await using (var processor = new FormFileProcessor())
        {
            List<FileDto> filesDto = processor.Process(dto.Files);

            var request = new UploadPetPhotoRequest(
                    id,
                    petid,
                    dto.Title,
                    filesDto);

            var result = await service.HandleAsync(request, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> Delete(
        [FromServices] DeleteVolunteerHandler handler,
        [FromServices] IValidator<DeleteVolunteerRequest> validator,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var request = new DeleteVolunteerRequest(id);

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
        [FromBody] UpdateVolunteerInfoRequestDto dto,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var request = new UpdateVolunteerInfoRequest(id, dto);

        var result = await handler.HandleAsync(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/payment")]
    public async Task<ActionResult<Guid>> PatchPayment(
        [FromServices] UpdateVolunteerPaymentHandler handler,
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerPaymentRequestDto dto,
        CancellationToken cancellationToken = default)
    {
        var request = new UpdateVolunteerPaymentRequest(id, dto);

        var result = await handler.HandleAsync(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }


    [HttpPut("{id:guid}/social")]
    public async Task<ActionResult<Guid>> PatchSocial(
        [FromServices] UpdateVolunteerSocialsHandler handler,
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerSocialsRequestDto dto,
        CancellationToken cancellationToken = default)
    {
        var request = new UpdateVolunteerSocialsRequest(id, dto);

        var result = await handler.HandleAsync(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}
