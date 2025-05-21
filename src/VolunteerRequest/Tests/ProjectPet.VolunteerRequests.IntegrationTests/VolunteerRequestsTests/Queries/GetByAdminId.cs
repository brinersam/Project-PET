using Microsoft.Extensions.DependencyInjection;
using ProjectPet.VolunteerRequests.IntegrationTests.Factories;
using ProjectPet.VolunteerRequests.IntegrationTests.VolunteerRequestsTests.Base;
using AutoFixture;
using FluentAssertions;
using ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Queries.GetByAdminIdPaginatedFiltered;
using ProjectPet.VolunteerRequests.Contracts.Dto;

namespace ProjectPet.VolunteerRequests.IntegrationTests.VolunteerRequestsTests.Queries;
public class GetByAdminId : VolunteerRequestsTestBase
{
    protected GetByAdminIdPaginatedFilteredHandler _sut;

    public GetByAdminId(VolunteerRequestsWebFactory factory) : base(factory)
    {
        _sut = _serviceScope.ServiceProvider.GetRequiredService<GetByAdminIdPaginatedFilteredHandler>();
    }

    [Fact]
    public async Task GetByAdmin_NoFilter_DefaultFilter()
    {
        // Arrange
        var admin1Id = _fixture.Create<Guid>();
        var admin2Id = _fixture.Create<Guid>();
        var volunteer1Admin1 = await SeedVolunteerRequestAndSetToReview(admin1Id, default);
        var volunteer2Admin1 = await SeedVolunteerRequestAndSetToRevisionRequired(admin1Id, default);

        var volunteer2Admin2 = await SeedVolunteerRequestAndSetToReview(admin2Id, default);

        var volunteer3AdminNONE = await SeedVolunteerRequestAsync();

        var query = new GetByAdminIdPaginatedFilteredQuery(admin1Id, new(null)) { Page = 0, RecordAmount = 99 };

        // Act
        var result = await _sut.HandleAsync(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var data = result.Value.Data;

        data.Should().HaveCount(1);
        data.Should().Contain(x => x.Id == volunteer1Admin1.Id);
        data.Should().Contain(x => x.Status == VolunteerRequestStatusDto.onReview);
    }

    [Fact]
    public async Task GetByAdmin_Filter_Works()
    {
        // Arrange
        var admin1Id = _fixture.Create<Guid>();
        var admin2Id = _fixture.Create<Guid>();
        var volunteer1Admin1 = await SeedVolunteerRequestAndSetToReview(admin1Id, default);
        var volunteer2Admin1 = await SeedVolunteerRequestAndSetToRevisionRequired(admin1Id, default);

        var volunteer2Admin2 = await SeedVolunteerRequestAndSetToReview(admin2Id, default);

        var volunteer3AdminNONE = await SeedVolunteerRequestAsync();

        var query = new GetByAdminIdPaginatedFilteredQuery(admin1Id, new(VolunteerRequestStatusDto.revisionRequired)) { Page = 0, RecordAmount = 99 };

        // Act
        var result = await _sut.HandleAsync(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var data = result.Value.Data;

        data.Should().HaveCount(1);
        data.Should().Contain(x => x.Id == volunteer2Admin1.Id);
        data.Should().Contain(x => x.Status == VolunteerRequestStatusDto.revisionRequired);
    }
}
