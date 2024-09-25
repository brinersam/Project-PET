using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<Guid>> Post([FromServices] CreateVolunteerHandler service, [FromBody] CreateVolunteerRequest dto)
        {
            ActionResult<Guid> result;
            try // TODO refactor to use Result
            {
                result = await service.HandleAsync(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }

            return Ok(result);
        }
    }
}
