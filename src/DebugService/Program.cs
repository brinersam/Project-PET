
using ProjectPet.AccountsModule.Infrastructure;
using ProjectPet.Framework.Authorization;
using ProjectPet.Web.MIddlewares;

namespace DebugService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddAuthModuleInfrastructure();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

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
