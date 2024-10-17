using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ProjectPet.API.Response;

namespace ProjectPet.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class CustomControllerBase : ControllerBase
    {
        public BadRequestObjectResult BadRequest<T>([ActionResultObjectValue] T[] error)
        {
            var envelope = Envelope.Error(error);
            return new BadRequestObjectResult(envelope);
        }
        public override BadRequestObjectResult BadRequest([ActionResultObjectValue] object? error)
            => BadRequest([error]);

        public override OkObjectResult Ok([ActionResultObjectValue] object? value)
        {
            var envelope = Envelope.Ok(value);
            return new OkObjectResult(envelope);
        }
    }
}
