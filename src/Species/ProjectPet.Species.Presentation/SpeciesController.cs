using Microsoft.AspNetCore.Mvc;
using ProjectPet.API.Extentions;
using ProjectPet.Application.Repositories;
using ProjectPet.Application.UseCases.AnimalSpecies.Commands.CreateBreed;
using ProjectPet.Application.UseCases.AnimalSpecies.Commands.CreateSpecies;
using ProjectPet.Application.UseCases.AnimalSpecies.Commands.DeleteBreed;
using ProjectPet.Application.UseCases.AnimalSpecies.Commands.DeleteSpecies;
using ProjectPet.Application.UseCases.AnimalSpecies.Queries.GetAllBreedsById;
using ProjectPet.Application.UseCases.AnimalSpecies.Queries.GetAllSpecies;
using ProjectPet.Framework;
using ProjectPet.SpeciesModule.Presentation.Requests;

namespace ProjectPet.SpeciesModule.Presentation;

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
        var cmd = request.ToCommand(id);
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

