using Microsoft.AspNetCore.Mvc;
using ProjectPet.API.Contracts.Species;
using ProjectPet.API.Extentions;
using ProjectPet.Application.UseCases.AnimalSpecies;
using ProjectPet.Application.UseCases.AnimalSpecies.CreateBreed;
using ProjectPet.Application.UseCases.AnimalSpecies.CreateSpecies;
using ProjectPet.Application.UseCases.AnimalSpecies.DeleteBreed;
using ProjectPet.Application.UseCases.AnimalSpecies.DeleteSpecies;

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
        [FromBody] CreateSpeciesRequestDto dto,
        CancellationToken cancellationToken = default)
    {
        var result = await service.HandleAsync(dto, cancellationToken);

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
        var request = new DeleteSpeciesRequest(id);
        var result = await service.HandleAsync(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [HttpPost("{id:guid}/breeds")]
    public async Task<IActionResult> PostBreed(
        [FromServices] CreateBreedsHandler service,
        [FromRoute] Guid id,
        [FromBody] CreateBreedsRequestDto dto,
        CancellationToken cancellationToken = default)
    {
        var request = new CreateBreedsRequest(id, dto.BreedName);
        var result = await service.HandleAsync(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}/breeds")]
    public async Task<ActionResult> DeleteBreed(
        [FromServices] DeleteBreedsHandler service,
        [FromBody] DeleteBreedsHandlerDto dto,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var request = new DeleteBreedsRequest(id, dto.BreedId);

        var result = await service.HandleAsync(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }
}
