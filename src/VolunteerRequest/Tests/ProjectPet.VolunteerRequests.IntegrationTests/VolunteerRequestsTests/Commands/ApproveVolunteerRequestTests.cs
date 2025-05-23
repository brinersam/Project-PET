﻿using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectPet.AccountsModule.Domain.Accounts;
using ProjectPet.VolunteerRequests.Application.Features.VolunteerRequests.Commands.Approve;
using ProjectPet.VolunteerRequests.Contracts.Dto;
using ProjectPet.VolunteerRequests.IntegrationTests.Factories;
using ProjectPet.VolunteerRequests.IntegrationTests.VolunteerRequestsTests.Base;

namespace ProjectPet.VolunteerRequests.IntegrationTests.VolunteerRequestsTests.Commands;
public class ApproveVolunteerRequestTests : VolunteerRequestsTestBase
{
    protected ApproveHandler _sut;

    public ApproveVolunteerRequestTests(VolunteerRequestsWebFactory factory) : base(factory)
    {
        _sut = _serviceScope.ServiceProvider.GetRequiredService<ApproveHandler>();
    }

    [Fact]
    public async Task Approve_VolunteerRequest_Success()
    {
        // Arrange
        var user = await SeedUserAsync();
        var volunteerRequest = await SeedVolunteerRequestAndSetToReview(null, user.Id);

        var cmd = new ApproveCommand(volunteerRequest.Id);

        // Act
        var result = await _sut.HandleAsync(cmd, default);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var volunteerRequestAssert = await _readDbContext.VolunteerRequests.AsNoTracking().FirstOrDefaultAsync();

        // request status is set to approved
        volunteerRequestAssert!.Status.Should().Be(VolunteerRequestStatusDto.approved);

        // volunteer data is created for user
        user.VolunteerData.Should().NotBeNull();

        // volunteer role is set for user, user still has member role
        var roles = await _userManager.GetRolesAsync(user);
        roles.Should().HaveCount(2);
        roles.Should().Contain(MemberAccount.ROLENAME);
        roles.Should().Contain(VolunteerAccount.ROLENAME);
    }

    [Fact]
    public async Task Approve_VolunteerRequest_CreateVolunteerAccountFailure_RequestUnchanged()
    {
        // Arrange
        _factory.IAccountsModuleContract_MakeUserVolunteerAsync_Failure();

        var user = await SeedUserAsync();
        var volunteerRequest = await SeedVolunteerRequestAndSetToReview(default, user.Id);

        var initialStatus = volunteerRequest!.Status;

        var cmd = new ApproveCommand(volunteerRequest.Id);

        // Act
        var result = await _sut.HandleAsync(cmd, default);

        // Assert
        result.IsSuccess.Should().BeFalse();

        var volunteerRequestAssert = await _readDbContext.VolunteerRequests.FirstOrDefaultAsync();

        // request status is unchanged
        volunteerRequestAssert!.Status.Should().Be((VolunteerRequestStatusDto)initialStatus);

        // volunteer data is not created
        user.VolunteerData.Should().BeNull();

        // volunteer role is unset set for user
        var roles = await _userManager.GetRolesAsync(user);
        roles.Should().HaveCount(1);
        roles.Should().Contain(MemberAccount.ROLENAME);
        roles.Should().NotContain(VolunteerAccount.ROLENAME);
    }
}

