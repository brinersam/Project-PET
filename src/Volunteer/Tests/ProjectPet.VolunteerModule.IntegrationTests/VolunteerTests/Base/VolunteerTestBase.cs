using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using ProjectPet.VolunteerModule.Application.Interfaces;
using ProjectPet.VolunteerModule.Contracts.Dto;
using ProjectPet.VolunteerModule.Domain.Models;
using ProjectPet.VolunteerModule.Infrastructure.Database;
using ProjectPet.VolunteerModule.IntegrationTests.Factories;
using System.Text;

namespace ProjectPet.VolunteerModule.IntegrationTests.VolunteerTests.Base;
public class VolunteerTestBase : IClassFixture<VolunteerWebFactory>, IAsyncLifetime
{
    protected readonly Fixture _fixture;
    protected readonly IReadDbContext _readDbContext;
    protected readonly WriteDbContext _writeDbContext;
    protected readonly VolunteerWebFactory _factory;
    protected readonly IServiceScope _serviceScope;
    public VolunteerTestBase(VolunteerWebFactory factory)
    {
        _factory = factory;
        _serviceScope = _factory.Services.CreateScope();
        _fixture = new Fixture();
        _readDbContext = _serviceScope.ServiceProvider.GetRequiredService<IReadDbContext>();
        _writeDbContext = _serviceScope.ServiceProvider.GetRequiredService<WriteDbContext>();

        SetupFixtures();
    }

    public Task InitializeAsync()
        => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        _serviceScope.Dispose();
        await _factory.ResetDatabaseAsync();
    }

    #region Data generation

    private void SetupFixtures()
    {
        _fixture.Register(() => new PhonenumberDto("123-456-78-99", "+4"));
        _fixture.Register<Stream>(() => new MemoryStream(Encoding.UTF8.GetBytes("whatever")));
    }

    protected async Task<Volunteer> SeedVolunteerAsync()
    {
        var volunteer = CreateVolunteer();
        await _writeDbContext.Volunteers.AddAsync(volunteer);
        await _writeDbContext.SaveChangesAsync();
        return volunteer;
    }

    protected async Task<(Volunteer volunteer, Pet pet)> SeedVolunteerWithPetAsync()
    {
        var volunteer = await SeedVolunteerAsync();

        var pet = CreatePet(volunteer);

        volunteer.AddPet(pet);

        _writeDbContext.Volunteers.Attach(volunteer);
        await _writeDbContext.SaveChangesAsync();

        return (volunteer, pet);
    }

    private Volunteer CreateVolunteer(int idx = 1)
    {
        return Volunteer.Create(
            Guid.NewGuid(),
            $"{idx}:fullName",
            $"{idx}gmail@email.com",
            $"{idx}:description",
            1,
            Phonenumber.Create("123-456-78-98", $"{idx}").Value)
            .Value;
    }

    private Pet CreatePet(Volunteer volunteer, int idx = 1)
    {
        var animalData = AnimalData.Create(Guid.NewGuid(), Guid.NewGuid());
        var phonenumber = Phonenumber.Create($"123-456-90-90", $"{idx}");
        var address = Address.Create("name", "street", "building", null, null, null, 25);
        var healthInfo = HealthInfo.Create("healthy", true, true, 1, 1);
        var orderingPosition = Position.Create(idx);
        var pet = Pet.Create(
            "Name",
            animalData.Value,
            "Description",
            "Coat",
            orderingPosition,
            healthInfo.Value,
            address.Value,
            phonenumber.Value,
            PetStatus.Looking_For_Home,
            DateOnly.FromDateTime(DateTime.Now),
            [],
            []);
        return pet.Value;
    }
    #endregion
}
