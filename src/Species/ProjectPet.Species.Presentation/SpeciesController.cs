using Microsoft.AspNetCore.Mvc;
using ProjectPet.Framework;
using ProjectPet.Framework.Authorization;
using ProjectPet.SpeciesModule.Application.Commands.CreateBreed;
using ProjectPet.SpeciesModule.Application.Commands.CreateSpecies;
using ProjectPet.SpeciesModule.Application.Commands.DeleteBreed;
using ProjectPet.SpeciesModule.Application.Commands.DeleteSpecies;
using ProjectPet.SpeciesModule.Application.Interfaces;
using ProjectPet.SpeciesModule.Application.Queries.GetAllBreedsById;
using ProjectPet.SpeciesModule.Application.Queries.GetAllSpecies;
using ProjectPet.SpeciesModule.Domain.Requests;

namespace ProjectPet.SpeciesModule.Presentation;

[Permission(PermissionCodes.SpeciesRead)]
public class SpeciesController : CustomControllerBase
{
    private readonly ISpeciesRepository _speciesRepository;

    public SpeciesController(ISpeciesRepository speciesRepository)
    {
        _speciesRepository = speciesRepository;
    }

    [HttpPost]
    public async Task<IActionResult> PostSpecies(
        [FromServices] CreateSpeciesHandler handler,
        [FromBody] CreateSpeciesCommand cmd,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.HandleAsync(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteSpecies(
        [FromServices] DeleteSpeciesHandler handler,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var request = new DeleteSpeciesCommand(id);
        var result = await handler.HandleAsync(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result);
    }

    [HttpPost("{id:guid}/breeds")]
    public async Task<IActionResult> PostBreed(
        [FromServices] CreateBreedsHandler handler,
        [FromRoute] Guid id,
        [FromBody] CreateBreedsRequest request,
        CancellationToken cancellationToken = default)
    {
        var cmd = CreateBreedsCommand.FromRequest(request, id);
        var result = await handler.HandleAsync(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpDelete("{speciesId:guid}/breeds")]
    public async Task<ActionResult> DeleteBreed(
        [FromServices] DeleteBreedsHandler handler,
        [FromBody] DeleteBreedRequest request,
        [FromRoute] Guid speciesId,
        CancellationToken cancellationToken = default)
    {
        var cmd = new DeleteBreedsCommand(speciesId, request.BreedId);

        var result = await handler.HandleAsync(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [Permission(PermissionCodes.SpeciesRead)]
    [HttpGet()]
    public async Task<IActionResult> GetAllSpecies(
        [FromServices] GetAllSpeciesHandler handler,
        [FromQuery] GetAllSpeciesRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllSpeciesQuery() { Page = request.Page, RecordAmount = request.Take };
        var result = await handler.HandleAsync(query, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpGet("{SpeciesId:guid}")]
    public async Task<IActionResult> GetAllBreedsById(
        [FromServices] GetAllBreedsByIdHandler handler,
        [FromRoute] Guid SpeciesId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllBreedsByIdQuery(SpeciesId);
        var result = await handler.HandleAsync(query, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}

