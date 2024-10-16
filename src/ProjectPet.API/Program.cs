using ProjectPet.Infrastructure;
using Serilog;
using ProjectPet.API.MIddlewares;
using ProjectPet.Application;
using ProjectPet.API;

var builder = WebApplication.CreateBuilder(args);

builder.AddSerilogLogger();

#region ASP 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion
#region App layers 
builder.AddInfrastructure();
builder.AddApplication();
#endregion

builder.Services.AddAutoValidation();

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
