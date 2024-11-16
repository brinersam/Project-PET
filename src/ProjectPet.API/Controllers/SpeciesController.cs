using Microsoft.AspNetCore.Mvc;
using ProjectPet.API.Extentions;
using ProjectPet.API.Requests.AnimalSpecies;
using ProjectPet.Application.Repositories;
using ProjectPet.Application.UseCases.AnimalSpecies.Commands.CreateBreed;
using ProjectPet.Application.UseCases.AnimalSpecies.Commands.CreateSpecies;
using ProjectPet.Application.UseCases.AnimalSpecies.Commands.DeleteBreed;
using ProjectPet.Application.UseCases.AnimalSpecies.Commands.DeleteSpecies;

namespace ProjectPet.API.Controllers;

public class SpeciesController : CustomControllerBase
{
    private readonly ISpeciesRepository _speciesRepository;

    public SpeciesController(ISpeciesRepository speciesRepository)
    {
        _speciesRepository = speciesRepository;
    }

    [HttpPost]
    public async Task<IActionResult> PostSpecies(
        [FromServices] CreateSpeciesHandler service,
        [FromBody] CreateSpeciesCommand cmd,
        CancellationToken cancellationToken = default)
    {
        var result = await service.HandleAsync(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteSpecies(
        [FromServices] DeleteSpeciesHandler service,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var request = new DeleteSpeciesCommand(id);
        var result = await service.HandleAsync(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [HttpPost("{id:guid}/breeds")]
    public async Task<IActionResult> PostBreed(
        [FromServices] CreateBreedsHandler service,
        [FromRoute] Guid id,
        [FromBody] CreateBreedsRequest request,
        CancellationToken cancellationToken = default)
    {
        var cmd = request.ToCommand(id);
        var result = await service.HandleAsync(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}/breeds")]
    public async Task<ActionResult> DeleteBreed(
        [FromServices] DeleteBreedsHandler service,
        [FromBody] DeleteBreedRequest request,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var cmd = new DeleteBreedsCommand(id, request.BreedId);

        var result = await service.HandleAsync(cmd, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }
}
