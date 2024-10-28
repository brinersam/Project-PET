using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProjectPet.API.Extentions;
using ProjectPet.Application.UseCases.Volunteers;
using ProjectPet.Application.UseCases.Volunteers.CreateVolunteer;
using ProjectPet.Application.UseCases.Volunteers.DeleteVolunteer;

namespace ProjectPet.API.Controllers;

public class SpeciesController : CustomControllerBase
{
    private readonly IVolunteerRepository _volunteerRepository;

    public SpeciesController(IVolunteerRepository volunteerRepository)
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
}
