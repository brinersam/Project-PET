using Microsoft.AspNetCore.Mvc;
using ProjectPet.API.Extentions;
using ProjectPet.API.Requests.Volunteers;
using ProjectPet.Application.UseCases.Volunteers.Queries.GetPetById;
using ProjectPet.Application.UseCases.Volunteers.Queries.GetPetsPaginated;

namespace ProjectPet.API.Controllers;

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
        var query = request.ToCommand();

        var result = await handler.HandleAsync(query, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}
