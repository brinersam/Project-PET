using Microsoft.AspNetCore.Mvc;
using ProjectPet.Application.UseCases.CreateVolunteer;
using ProjectPet.Infrastructure.Repositories;

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
        public async Task<ActionResult> Post([FromBody] CreateVolunteerRequest dto)
        {
            var service = new CreateVolunteerHandler(_volunteerRepository);

            var result = await service.HandleAsync(dto);

            return Ok(result);
        }
    }
}
