using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProjectPet.Application.UseCases;
using ProjectPet.Application.UseCases.CreateVolunteer;

namespace ProjectPet.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VolunteerController : ControllerBase
    {
        private readonly IVolunteerRepository _volunteerRepository;

        public VolunteerController(IVolunteerRepository volunteerRepository)
        {
            _volunteerRepository = volunteerRepository;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Post(
            [FromServices] CreateVolunteerHandler service,
            [FromServices] IValidator<CreateVolunteerRequestDto> validator,
            [FromBody] CreateVolunteerRequestDto dto,
            CancellationToken cancellationToken = default)
        {
            var validatorResult = await validator.ValidateAsync(dto, cancellationToken);
            if (validatorResult.IsValid == false)
                return BadRequest(validatorResult.Errors);

            var result = await service.HandleAsync(dto,cancellationToken);

            if (result.IsFailure)
                return BadRequest(result.Error.Message);
            // todo refactor to handle different error codes

            return Ok(result.Value);
        }
    }
}
