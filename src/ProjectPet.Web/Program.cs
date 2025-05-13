using ProjectPet.AccountsModule.Infrastructure;
using ProjectPet.AccountsModule.Presentation;
using ProjectPet.FileManagement.Infrastructure;
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

builder.AddFileManagementInfrastructure();

builder.AddAuthModuleHandlers();
builder.AddAuthModuleInfrastructure();

builder.AddVolunteerRequestsModuleHandlers();
builder.AddVolunteerRequestModuleInfrastructure();
#endregion

builder.Services.AddValidation();

var app = builder.Build();

await app.SeedDatabaseAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomExceptionHandler();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program;