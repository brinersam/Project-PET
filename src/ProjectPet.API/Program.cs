using ProjectPet.Infrastructure;
using ProjectPet.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Serilog;
using Serilog.Events;
using ProjectPet.API.MIddlewares;
using ProjectPet.Application.UseCases.Volunteers;

var builder = WebApplication.CreateBuilder(args);

string seqConnectionString = builder.Configuration["CStrings:Seq"] 
    ?? throw new ArgumentNullException("CStrings:Seq");

Log.Logger = new LoggerConfiguration()
.WriteTo.Console()
.WriteTo.Debug()
    .WriteTo.Seq(seqConnectionString)
    .Enrich.WithThreadId()
    .Enrich.WithEnvironmentName()
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
    .CreateLogger();

builder.Services.AddSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ApplicationDbContext>();
builder.Services.AddScoped<IVolunteerRepository, VolunteerRepository>();

builder.Services.AddScoped<CreateVolunteerHandler>();
builder.Services.AddScoped<UpdateVolunteerInfoHandler>();
builder.Services.AddScoped<UpdateVolunteerPaymentHandler>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(typeof(IVolunteerRepository).Assembly);

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
