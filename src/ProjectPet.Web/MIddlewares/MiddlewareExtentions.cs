namespace ProjectPet.Web.MIddlewares;

public static class MiddlewareExtentions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }
}
