
using DebugService.BuilderAppExtensions;
using ProjectPet.Framework.Authorization;
using ProjectPet.Framework.UserData;

namespace DebugService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddAuth();

        builder.AddRabbitMQ();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHttpClient();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseMiddleware<ScopedUserDataMiddleware>();
        app.UseAuthorization();

        app.MapGet("/debugep", [Permission(PermissionCodes.AdminMasterkey)] (HttpContext httpContext) =>
        {
            return "hi";
        })
        .WithName("DebugEP")
        .WithOpenApi();

        app.Run();
    }
}
