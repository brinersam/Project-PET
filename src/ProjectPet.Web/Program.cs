using ProjectPet.AccountsModule.Infrastructure;
using ProjectPet.AccountsModule.Presentation;
using ProjectPet.DiscussionsModule.Infrastructure;
using ProjectPet.DiscussionsModule.Presentation;
using ProjectPet.FileService.Communication.Extensions;
using ProjectPet.SpeciesModule.Infrastructure;
using ProjectPet.SpeciesModule.Presentation;
using ProjectPet.VolunteerModule.Infrastructure;
using ProjectPet.VolunteerModule.Presentation;
using ProjectPet.VolunteerRequests.Infrastructure;
using ProjectPet.VolunteerRequests.Presentation;
using ProjectPet.Web;
using ProjectPet.Web.Extentions;
using ProjectPet.Web.MIddlewares;
using Serilog;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(
        policy => policy
            .WithOrigins("http://localhost:5173", "https://localhost:8081/", "http://localhost:8080/")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
    )
);

builder.ConfigureDbCstring();

builder.AddSerilogLogger();

#region ASP 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.AddAuthenticatedSwaggerGen();
#endregion

#region App modules 
builder.AddVolunteerModuleHandlers();
builder.AddVolunteerModuleInfrastructure();

builder.AddSpeciesModuleHandlers();
builder.AddSpeciesModuleInfrastructure();

builder.AddAuthModuleHandlers();
builder.AddAuthModuleInfrastructure();

builder.AddVolunteerRequestsModuleHandlers();
builder.AddVolunteerRequestModuleInfrastructure();

builder.AddDiscussionModuleHandlers();
builder.AddDiscussionsModuleInfrastructure();
#endregion

#region Microservice clients
builder.AddFileServiceHttpClient();
#endregion

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.AddValidation();
builder.Services.AddScoped<ScopedUserDataMiddleware>();

var app = builder.Build();

await app.SeedDatabaseAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomExceptionHandler();

app.UseSerilogRequestLogging();


// app.UseHttpsRedirection(); uncomment after configuring nginx support for https
app.UseCors();

app.UseAuthentication();
app.UseMiddleware<ScopedUserDataMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program;