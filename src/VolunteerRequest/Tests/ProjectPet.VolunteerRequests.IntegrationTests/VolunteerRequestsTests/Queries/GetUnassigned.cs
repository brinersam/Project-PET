using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Queries.GetUnassignedPaginated;
using ProjectPet.VolunteerRequests.IntegrationTests.Factories;
using ProjectPet.VolunteerRequests.IntegrationTests.VolunteerRequestsTests.Base;

namespace ProjectPet.VolunteerRequests.IntegrationTests.VolunteerRequestsTests.Queries;
public class GetUnassigned : VolunteerRequestsTestBase
{
    protected GetUnassignedPaginatedHandler _sut;

    public GetUnassigned(VolunteerRequestsWebFactory factory) : base(factory)
    {
        _sut = _serviceScope.ServiceProvider.GetRequiredService<GetUnassignedPaginatedHandler>();
    }

    [Fact]
    public async Task Admin1Userx1_Admin2Userx1_Userx1_GetUnassigned_GetsUnassigned()
    {
        // Arrange
        var admin1Id = _fixture.Create<Guid>();
        var admin2Id = _fixture.Create<Guid>();
        var volunteer1Admin1 = await SeedVolunteerRequestAndSetToReview(admin1Id, default);
        var volunteer2Admin2 = await SeedVolunteerRequestAndSetToReview(admin2Id, default);
        var volunteer3AdminNONE = await SeedVolunteerRequestAsync();

        var query = new GetUnassignedPaginatedQuery() {Page = 0, RecordAmount = 99 };

        // Act
        var result = await _sut.HandleAsync(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var data = result.Value.Data;

        data.Should().HaveCount(1);
        data.Should().Contain(x => x.Id == volunteer3AdminNONE.Id);
    }
}
