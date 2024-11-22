using ProjectPet.FileManagement.Infrastructure;
using ProjectPet.SpeciesModule.Infrastructure;
using ProjectPet.SpeciesModule.Presentation;
using ProjectPet.VolunteerModule.Infrastructure;
using ProjectPet.VolunteerModule.Presentation;
using ProjectPet.Web;
using ProjectPet.Web.MIddlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureDbCstring();

builder.AddSerilogLogger();

#region ASP 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion
#region App modules 
builder.AddVolunteerModuleHandlers();
builder.AddVolunteerModuleInfrastructure();

builder.AddSpeciesModuleHandlers();
builder.AddSpeciesModuleInfrastructure();

builder.AddFileManagementInfrastructure();
#endregion

builder.Services.AddValidation();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomExceptionHandler();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
