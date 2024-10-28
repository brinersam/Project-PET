using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProjectPet.API.Extentions;
using ProjectPet.Application.UseCases.Volunteers;
using ProjectPet.Application.UseCases.Volunteers.CreateVolunteer;
using ProjectPet.Application.UseCases.Volunteers.DeleteVolunteer;
using ProjectPet.Application.UseCases.Volunteers.UpdateVolunteerInfo;
using ProjectPet.Application.UseCases.Volunteers.UpdateVolunteerPayment;
using ProjectPet.Application.UseCases.Volunteers.UpdateVolunteerSocials;

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
        [FromServices] CreateVolunteerHandler service,
        [FromBody] CreateVolunteerRequestDto dto,
        CancellationToken cancellationToken = default)
    {
        var result = await service.HandleAsync(dto, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> Delete(
        [FromServices] DeleteVolunteerHandler service,
        [FromServices] IValidator<DeleteVolunteerRequest> validator,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var request = new DeleteVolunteerRequest(id);

        var validatorRes = await validator.ValidateAsync(request, cancellationToken);
        if (validatorRes.IsValid == false)
            return BadRequest(validatorRes.Errors);

        var result = await service.HandleAsync(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPatch("{id:guid}/main")]
    public async Task<ActionResult<Guid>> PatchInfo(
        [FromServices] UpdateVolunteerInfoHandler service,
        [FromBody] UpdateVolunteerInfoRequestDto dto,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var request = new UpdateVolunteerInfoRequest(id, dto);

        var result = await service.HandleAsync(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/payment")]
    public async Task<ActionResult<Guid>> PatchPayment(
        [FromServices] UpdateVolunteerPaymentHandler service,
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerPaymentRequestDto dto,
        CancellationToken cancellationToken = default)
    {
        var request = new UpdateVolunteerPaymentRequest(id, dto);

        var result = await service.HandleAsync(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }


    [HttpPut("{id:guid}/social")]
    public async Task<ActionResult<Guid>> PatchSocial(
        [FromServices] UpdateVolunteerSocialsHandler service,
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerSocialsRequestDto dto,
        CancellationToken cancellationToken = default)
    {
        var request = new UpdateVolunteerSocialsRequest(id, dto);

        var result = await service.HandleAsync(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}
