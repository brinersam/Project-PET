using AutoFixture;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.AccountsModule.Domain.Accounts;
using ProjectPet.AccountsModule.Infrastructure.Database;
using ProjectPet.VolunteerRequests.Domain.Models;
using ProjectPet.VolunteerRequests.Infrastructure.Database;
using ProjectPet.VolunteerRequests.IntegrationTests.Factories;
using ReadDbContextACC = ProjectPet.AccountsModule.Infrastructure.Database.ReadDbContext;
using ReadDbContextVR = ProjectPet.VolunteerRequests.Infrastructure.Database.ReadDbContext;

namespace ProjectPet.VolunteerRequests.IntegrationTests.VolunteerRequestsTests.Base;
public class VolunteerRequestsTestBase : IClassFixture<VolunteerRequestsWebFactory>, IAsyncLifetime
{
    protected readonly Fixture _fixture;
    protected readonly ReadDbContextVR _readDbContextVR;
    protected readonly ReadDbContextACC _readDbContextACC;
    protected readonly WriteDbContext _writeDbContext;
    protected readonly AuthDbContext _authDbContext;
    protected readonly UserManager<User> _userManager;
    protected readonly VolunteerRequestsWebFactory _factory;
    protected readonly IServiceScope _serviceScope;
    public VolunteerRequestsTestBase(VolunteerRequestsWebFactory factory)
    {
        _factory = factory;
        _serviceScope = _factory.Services.CreateScope();
        _fixture = new Fixture();
        _readDbContextVR = _serviceScope.ServiceProvider.GetRequiredService<ReadDbContextVR>();
        _readDbContextACC = _serviceScope.ServiceProvider.GetRequiredService<ReadDbContextACC>();
        _writeDbContext = _serviceScope.ServiceProvider.GetRequiredService<WriteDbContext>();
        _authDbContext = _serviceScope.ServiceProvider.GetRequiredService<AuthDbContext>();
        _userManager = _serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();

        _factory.ToggleMocks.SetMockProviders(_serviceScope.ServiceProvider);
        SetupFixtures();
    }

    public Task InitializeAsync()
        => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        _factory.ToggleMocks.ResetMocks();
        _serviceScope.Dispose();
        await _factory.ResetDatabaseAsync();
    }

    #region Data generation

    private void SetupFixtures()
    {
        //_fixture.Register(() => new PhonenumberDto("123-456-78-99", "+4"));
        //_fixture.Register<Stream>(() => new MemoryStream(Encoding.UTF8.GetBytes("whatever")));
    }

    protected async Task<User> SeedUserAsync()
    {
        var user = await User.CreateMemberAsync(
            _userManager,
            _fixture.Create<string>(),
            $"A{_fixture.Create<string>()}123#",
            $"{_fixture.Create<string>()}@mail.com",
            _fixture.Create<MemberAccount>()
            );

        return user.Value;
    }

    protected async Task<VolunteerRequest> SeedVolunteerRequestAsync(Guid userId = default, Action<VolunteerRequest> action = null!)
    {
        var volunteerReq = CreateVolunteerRequest(userId);

        action?.Invoke(volunteerReq);

        await _writeDbContext.VolunteerRequests.AddAsync(volunteerReq);
        await _writeDbContext.SaveChangesAsync();
        return volunteerReq;
    }
    protected async Task<VolunteerRequest> SeedVolunteerRequestAndSetToReview(Guid? adminId, Guid? userId)
    {
        adminId ??= _fixture.Create<Guid>();
        userId ??= _fixture.Create<Guid>();

        var volunteerRequest = await SeedVolunteerRequestAsync((Guid)userId, x => x.BeginReview((Guid)adminId));

        await _writeDbContext.SaveChangesAsync();

        return volunteerRequest;
    }

    protected async Task<VolunteerRequest> SeedVolunteerRequestAndSetToRevisionRequired(Guid? adminId, Guid? userId)
    {
        adminId ??= _fixture.Create<Guid>();
        userId ??= _fixture.Create<Guid>();

        var volunteerRequest = await SeedVolunteerRequestAsync((Guid)userId, x =>
        {
            x.BeginReview((Guid)adminId);
            x.RequestRevision("revision-required");
        });

        await _writeDbContext.SaveChangesAsync();

        return volunteerRequest;
    }

    private VolunteerRequest CreateVolunteerRequest(Guid userId = default)
    {
        return VolunteerRequest.Create(
            userId == default ? _fixture.Create<Guid>() : userId,
            _fixture.Create<Guid>(),
            _fixture.Create<VolunteerAccountData>()
        ).Value;
    }
    #endregion
}
