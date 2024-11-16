using ProjectPet.API.Response;
using System.Net;

namespace ProjectPet.API.MIddlewares;

public class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

    public CustomExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<CustomExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception was caught!: {error} {stacktrace}", ex.Message, ex.StackTrace);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var err = Error.Failure("exception", ex.Message);

            await context.Response.WriteAsJsonAsync(Envelope.Error([err]));
        }
    }
}
