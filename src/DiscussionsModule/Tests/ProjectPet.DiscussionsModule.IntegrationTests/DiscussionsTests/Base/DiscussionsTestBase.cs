using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute.ReceivedExtensions;
using ProjectPet.DiscussionsModule.Application.Interfaces;
using ProjectPet.DiscussionsModule.Domain.Models;
using ProjectPet.DiscussionsModule.Infrastructure.Database;
using ProjectPet.DiscussionsModule.IntegrationTests.Factories;

namespace ProjectPet.DiscussionsModule.IntegrationTests.DiscussionsTests.Base;
public class DiscussionsTestBase : IClassFixture<DiscussionsTestWebFactory>, IAsyncLifetime
{
    protected readonly Fixture _fixture;
    protected readonly IReadDbContext _readDbContext;
    protected readonly WriteDbContext _writeDbContext;
    protected readonly IDiscussionsRepository _repository;
    protected readonly DiscussionsTestWebFactory _factory;
    protected readonly IServiceScope _serviceScope;
    public DiscussionsTestBase(DiscussionsTestWebFactory factory)
    {
        _factory = factory;
        _serviceScope = _factory.Services.CreateScope();
        _fixture = new Fixture();
        _readDbContext = _serviceScope.ServiceProvider.GetRequiredService<IReadDbContext>();
        _writeDbContext = _serviceScope.ServiceProvider.GetRequiredService<WriteDbContext>();
        _repository = _serviceScope.ServiceProvider.GetRequiredService<IDiscussionsRepository>();

        //_factory.SetMockProviders(_serviceScope.ServiceProvider);
        SetupFixtures();
    }

    public Task InitializeAsync()
        => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        //_factory.ResetMocks();
        _serviceScope.Dispose();
        await _factory.ResetDatabaseAsync();
    }

    #region Data generation

    private void SetupFixtures()
    {
        //_fixture.Register(() => new PhonenumberDto("123-456-78-99", "+4"));
        //_fixture.Register<Stream>(() => new MemoryStream(Encoding.UTF8.GetBytes("whatever")));
    }

    protected async Task<Discussion> SeedDiscussionAsync(Guid? entityId = null, Action<Discussion> acts = null, params Guid[] userIds)
    {
        entityId ??= _fixture.Create<Guid>();
        if (userIds.Length <= 0)
            userIds = [_fixture.Create<Guid>(), _fixture.Create<Guid>()];

        var discussionRes = Discussion.Create(
            (Guid)entityId,
            userIds);

        acts?.Invoke(discussionRes.Value); 

        await _repository.AddAsync(discussionRes.Value);

        return discussionRes.Value;
    }

    //protected async Task<VolunteerRequest> SeedVolunteerRequestAsync(Guid userId = default, Action<VolunteerRequest> action = null!)
    //{
    //    var volunteerReq = CreateVolunteerRequest(userId);

    //    action?.Invoke(volunteerReq);

    //    await _writeDbContext.VolunteerRequests.AddAsync(volunteerReq);
    //    await _writeDbContext.SaveChangesAsync();
    //    return volunteerReq;
    //}
    //protected async Task<VolunteerRequest> SeedVolunteerRequestAndSetToReview(Guid? adminId, Guid? userId)
    //{
    //    adminId ??= _fixture.Create<Guid>();
    //    userId ??= _fixture.Create<Guid>();

    //    var volunteerRequest = await SeedVolunteerRequestAsync((Guid)userId, x => x.BeginReview((Guid)adminId));

    //    await _writeDbContext.SaveChangesAsync();

    //    return volunteerRequest;
    //}

    //protected async Task<VolunteerRequest> SeedVolunteerRequestAndSetToRevisionRequired(Guid? adminId, Guid? userId)
    //{
    //    adminId ??= _fixture.Create<Guid>();
    //    userId ??= _fixture.Create<Guid>();

    //    var volunteerRequest = await SeedVolunteerRequestAsync((Guid)userId, x =>
    //    {
    //        x.BeginReview((Guid)adminId);
    //        x.RequestRevision("revision-required");
    //    });

    //    await _writeDbContext.SaveChangesAsync();

    //    return volunteerRequest;
    //}

    //private VolunteerRequest CreateVolunteerRequest(Guid userId = default)
    //{
    //    return VolunteerRequest.Create(
    //        userId == default ? _fixture.Create<Guid>() : userId,
    //        _fixture.Create<Guid>(),
    //        _fixture.Create<VolunteerAccountData>()
    //    ).Value;
    //}
    #endregion
}
