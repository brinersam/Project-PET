using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using ProjectPet.AccountsModule.Infrastructure.Database;
using ProjectPet.Core.Database;
using ProjectPet.DiscussionsModule.Contracts;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.SharedKernel.Exceptions;
using ProjectPet.VolunteerRequests.Application.Interfaces;
using ProjectPet.VolunteerRequests.Contracts.Events;
using ProjectPet.VolunteerRequests.Domain.Models;
using ProjectPet.VolunteerRequests.IntegrationTests.ToggleMocks;
using ProjectPet.VolunteerRequests.IntegrationTests.ToggleMocks.Misc;
using ProjectPet.Web;
using DiscussionsWriteDbContext = ProjectPet.DiscussionsModule.Infrastructure.Database.WriteDbContext;
using ReadDbContextACC = ProjectPet.AccountsModule.Infrastructure.Database.ReadDbContext;
using ReadDbContextVR = ProjectPet.VolunteerRequests.Infrastructure.Database.ReadDbContext;
using VolunteerWriteDbContext = ProjectPet.VolunteerRequests.Infrastructure.Database.WriteDbContext;

namespace ProjectPet.VolunteerRequests.IntegrationTests.Factories;
public class VolunteerRequestsWebFactory : IntegrationTestWebFactoryBase
{
    public ToggleMockContainer ToggleMocks = new()
    {
        [typeof(DiscussionModuleContractToggleMock)] = new DiscussionModuleContractToggleMock(),
        [typeof(VolunteerRequestRepositoryToggleMock)] = new VolunteerRequestRepositoryToggleMock(),
        [typeof(CreateDiscussionEventHandlerToggleMock)] = new CreateDiscussionEventHandlerToggleMock(),
    };

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.RemoveAll<IDatabaseSeeder>();
        services.AddScoped<ReadDbContextACC>();
        services.AddScoped<ReadDbContextVR>();

        ToggleMocks.InjectToggleMocks(services);
        AddMessageQueue(services);

        base.ConfigureServices(services);
    }

    private void AddMessageQueue(IServiceCollection services)
    {
        services.AddMassTransitTestHarness(options => 
        {
            options.SetKebabCaseEndpointNameFormatter();
            RegisterServices.AddModuleConsumers(options);
        });
    }

    protected override IEnumerable<(Type DbContextType, string Schema)> IncludedDBContextsAndSchemas()
        => [
            (typeof(VolunteerWriteDbContext), "volunteer-requests"),
            (typeof(DiscussionsWriteDbContext), "discussion-module"),
            (typeof(AuthDbContext), "auth"),
           ];

    #region Mock methods
    public void CreateDiscussionEventHandlerMock_Handle_ThrowException()
    {
        var toggleMock = ToggleMocks.GetMock<CreateDiscussionEventHandlerToggleMock>();
        toggleMock.MockFunction(nameof(CreateDiscussionEventHandlerToggleMock.Handle));
        toggleMock.Mock
            .Handle(
                Arg.Any<VolunteerRequest_WasSetToReview_Event>(), Arg.Any<CancellationToken>()
            )
            .Throws(new DomainEventException("debug.exception"));
    }

    public Error IDiscussionModuleContractMock_CreateDiscussionAsync_Failure()
    {
        var toggleMock = ToggleMocks.GetMock<DiscussionModuleContractToggleMock>();

        var error = Error.Failure("debug.fail", "debugfail");

        toggleMock.MockFunction(nameof(IDiscussionModuleContract.CreateDiscussionAsync));
        toggleMock.Mock
            .CreateDiscussionAsync(
                Arg.Any<Guid>(), Arg.Any<IEnumerable<Guid>>()
            )
            .Returns(error);
        return error;
    }

    //[Obsolete("We now use rabbitMq instead of contracts")]
    //public Error IAccountsModuleContract_MakeUserVolunteerAsync_Failure()
    //{
    //    var error = Error.Failure("debug.fail", "debugfail");

    //    _accountModuleContractMock.MockFunction(nameof(IAccountsModuleContract.MakeUserVolunteerAsync));
    //    _accountModuleContractMock.Mock
    //        .MakeUserVolunteerAsync(
    //            Arg.Any<Guid>(), Arg.Any<VolunteerAccountDto>(), Arg.Any<CancellationToken>()
    //        )
    //        .Returns(error);
    //    return error;
    //}

    public Error IVolunteerRequestRepository_Save_Fail()
    {
        var toggleMock = ToggleMocks.GetMock<VolunteerRequestRepositoryToggleMock>();

        var error = Error.Failure("debug.fail", "debugfail");

        toggleMock.MockFunction(nameof(IVolunteerRequestRepository.Save));
        toggleMock.Mock
            .Save(
                Arg.Any<VolunteerRequest>(), Arg.Any<CancellationToken>()
            )
            .Returns(error);
        return error;
    }
    #endregion
}
