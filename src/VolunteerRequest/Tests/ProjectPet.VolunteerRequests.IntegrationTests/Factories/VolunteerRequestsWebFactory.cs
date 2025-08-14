using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using ProjectPet.AccountsModule.Contracts;
using ProjectPet.AccountsModule.Infrastructure.Database;
using ProjectPet.Core.Database;
using ProjectPet.DiscussionsModule.Contracts;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.SharedKernel.Exceptions;
using ProjectPet.SharedKernel.SharedDto;
using ProjectPet.VolunteerRequests.Application.Interfaces;
using ProjectPet.VolunteerRequests.Contracts.Events;
using ProjectPet.VolunteerRequests.Domain.Models;
using ProjectPet.VolunteerRequests.IntegrationTests.ToggleMocks;
using DiscussionsWriteDbContext = ProjectPet.DiscussionsModule.Infrastructure.Database.WriteDbContext;
using VolunteerWriteDbContext = ProjectPet.VolunteerRequests.Infrastructure.Database.WriteDbContext;

namespace ProjectPet.VolunteerRequests.IntegrationTests.Factories;
public class VolunteerRequestsWebFactory : IntegrationTestWebFactoryBase
{
    protected DiscussionModuleContractToggleMock _discussionModuleContractMock = new DiscussionModuleContractToggleMock();
    protected AccountsModuleContractToggleMock _accountModuleContractMock = new AccountsModuleContractToggleMock();
    protected VolunteerRequestRepositoryToggleMock _volunteerRequestRepository = new VolunteerRequestRepositoryToggleMock();
    protected CreateDiscussionEventHandlerToggleMock _createDiscussionEventHandlerToggleMock = new CreateDiscussionEventHandlerToggleMock();
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.RemoveAll<IDatabaseSeeder>();
        services.Replace(ServiceDescriptor.Scoped(typeof(IDiscussionModuleContract), _ => _discussionModuleContractMock));
        services.Replace(ServiceDescriptor.Scoped(typeof(IAccountsModuleContract), _ => _accountModuleContractMock));
        services.Replace(ServiceDescriptor.Scoped(typeof(IVolunteerRequestRepository), _ => _volunteerRequestRepository));
        services.Replace(ServiceDescriptor.Scoped(typeof(INotificationHandler<VolunteerRequest_WasSetToReview_Event>), _ => _createDiscussionEventHandlerToggleMock));
        base.ConfigureServices(services);
    }

    protected override IEnumerable<(Type DbContextType, string Schema)> IncludedDBContextsAndSchemas()
        => [
            (typeof(VolunteerWriteDbContext), "volunteer-requests"),
            (typeof(DiscussionsWriteDbContext), "discussion-module"),
            (typeof(AuthDbContext), "auth")
           ];

    #region Mock methods
    public void SetMockProviders(IServiceProvider provider)
    {
        _discussionModuleContractMock.SetProvider(provider);
        _accountModuleContractMock.SetProvider(provider);
        _volunteerRequestRepository.SetProvider(provider);
        _createDiscussionEventHandlerToggleMock.SetProvider(provider);
    }

    public void ResetMocks()
    {
        _discussionModuleContractMock.Reset();
        _accountModuleContractMock.Reset();
        _volunteerRequestRepository.Reset();
        _createDiscussionEventHandlerToggleMock.Reset();

    }

    public void CreateDiscussionEventHandlerMock_Handle_ThrowException()
    {
        _createDiscussionEventHandlerToggleMock.MockFunction(nameof(CreateDiscussionEventHandlerToggleMock.Handle));
        _createDiscussionEventHandlerToggleMock.Mock
            .Handle(
                Arg.Any<VolunteerRequest_WasSetToReview_Event>(), Arg.Any<CancellationToken>()
            )
            .Throws(new DomainEventException("debug.exception"));
    }

    public Error IDiscussionModuleContractMock_CreateDiscussionAsync_Failure()
    {
        var error = Error.Failure("debug.fail", "debugfail");

        _discussionModuleContractMock.MockFunction(nameof(IDiscussionModuleContract.CreateDiscussionAsync));
        _discussionModuleContractMock.Mock
            .CreateDiscussionAsync(
                Arg.Any<Guid>(), Arg.Any<IEnumerable<Guid>>()
            )
            .Returns(error);
        return error;
    }

    public Error IAccountsModuleContract_MakeUserVolunteerAsync_Failure()
    {
        var error = Error.Failure("debug.fail", "debugfail");

        _accountModuleContractMock.MockFunction(nameof(IAccountsModuleContract.MakeUserVolunteerAsync));
        _accountModuleContractMock.Mock
            .MakeUserVolunteerAsync(
                Arg.Any<Guid>(), Arg.Any<VolunteerAccountDto>(), Arg.Any<CancellationToken>()
            )
            .Returns(error);
        return error;
    }

    public Error IVolunteerRequestRepository_Save_Fail()
    {
        var error = Error.Failure("debug.fail", "debugfail");

        _volunteerRequestRepository.MockFunction(nameof(IVolunteerRequestRepository.Save));
        _volunteerRequestRepository.Mock
            .Save(
                Arg.Any<VolunteerRequest>(), Arg.Any<CancellationToken>()
            )
            .Returns(error);
        return error;
    }
    #endregion
}
