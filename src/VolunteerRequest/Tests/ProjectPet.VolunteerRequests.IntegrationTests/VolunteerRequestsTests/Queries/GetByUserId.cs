using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Queries.GetByUserIdPaginatedFiltered;
using ProjectPet.VolunteerRequests.Contracts.Dto;
using ProjectPet.VolunteerRequests.Domain.Models;
using ProjectPet.VolunteerRequests.IntegrationTests.Factories;
using ProjectPet.VolunteerRequests.IntegrationTests.VolunteerRequestsTests.Base;

namespace ProjectPet.VolunteerRequests.IntegrationTests.VolunteerRequestsTests.Queries;
public class GetByUserId : VolunteerRequestsTestBase
{
    protected GetByUserIdPaginatedFilteredHandler _sut;

    public GetByUserId(VolunteerRequestsWebFactory factory) : base(factory)
    {
        _sut = _serviceScope.ServiceProvider.GetRequiredService<GetByUserIdPaginatedFilteredHandler>();
    }

    [Fact]
    public async Task GetByUser_NoFilter_ReturnsEveryRequest()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();
        var request1 = await SeedVolunteerRequestAsync(userId);
        var request2 = await SeedVolunteerRequestAndSetToReview(default, userId);

        var query = new GetByUserIdPaginatedFilteredQuery(userId, new(null)) { Page = 0, RecordAmount = 99 };

        // Act
        var result = await _sut.HandleAsync(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var data = result.Value.Data;

        data.Should().HaveCount(2);
        data.Should().Contain(x => x.Id == request1.Id && x.Status == VolunteerRequestStatusDto.submitted);
        data.Should().Contain(x => x.Id == request2.Id && x.Status == VolunteerRequestStatusDto.onReview);
    }

    [Fact]
    public async Task GetByUser_Filter_Filtered()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();

        var request1 = await SeedVolunteerRequestAsync(userId);
        var request2 = await SeedVolunteerRequestAndSetToReview(default, userId);

        var query = new GetByUserIdPaginatedFilteredQuery(userId, new(VolunteerRequestStatusDto.onReview)) { Page = 0, RecordAmount = 99 };

        // Act
        var result = await _sut.HandleAsync(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var data = result.Value.Data;

        data.Should().HaveCount(1);
        data.Should().Contain(x => x.Id == request2.Id && x.Status == VolunteerRequestStatusDto.onReview);
    }
}
