using Microsoft.AspNetCore.Mvc;
using ProjectPet.Framework;
using ProjectPet.VolunteerModule.Application.Features.Pets.Queries.GetPetById;
using ProjectPet.VolunteerModule.Application.Features.Pets.Queries.GetPetsPaginated;
using ProjectPet.VolunteerModule.Contracts.Requests;

namespace ProjectPet.VolunteerModule.Presentation.Pets;

public class PetController : CustomControllerBase
{
    [HttpGet("{petid:guid}")]
    public async Task<ActionResult<Guid>> GetPetById(
        [FromServices] GetPetByIdHandler handler,
        [FromRoute] Guid petid,
        CancellationToken cancellationToken = default)
    {
        var query = new GetPetByIdQuery(petid);

        var result = await handler.HandleAsync(query, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpGet()]
    public async Task<ActionResult<Guid>> GetPetsPaginated(
        [FromServices] GetPetsPaginatedHandler handler,
        [FromQuery] GetPetsPaginatedRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = GetPetsPaginatedQuery.FromRequest(request);

        var result = await handler.HandleAsync(query, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}
