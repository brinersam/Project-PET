using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProjectPet.Core.Abstractions;
using ProjectPet.DiscussionsModule.Infrastructure.Database;

namespace ProjectPet.DiscussionsModule.IntegrationTests.Factories;
public class DiscussionsTestWebFactory : IntegrationTestWebFactoryBase
{
    //protected DiscussionModuleContractToggleMock _discussionModuleContractMock = new DiscussionModuleContractToggleMock();
    //protected AccountsModuleContractToggleMock _accountModuleContractMock = new AccountsModuleContractToggleMock();

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.RemoveAll<IDatabaseSeeder>();
        //services.Replace(ServiceDescriptor.Scoped(typeof(IDiscussionModuleContract), _ => _discussionModuleContractMock));
        //services.Replace(ServiceDescriptor.Scoped(typeof(IAccountsModuleContract), _ => _accountModuleContractMock));
        base.ConfigureServices(services);
    }

    protected override IEnumerable<(Type DbContextType, string Schema)> IncludedDBContextsAndSchemas()
        => [
            (typeof(WriteDbContext), "discussion-module")
           ];

    #region Mock methods
    //public void SetMockProviders(IServiceProvider provider)
    //{
    //    _discussionModuleContractMock.SetProvider(provider);
    //    _accountModuleContractMock.SetProvider(provider);
    //}

    //public void ResetMocks()
    //{
    //    _discussionModuleContractMock.Reset();
    //    _accountModuleContractMock.Reset();
    //}

    //public Error IDiscussionModuleContractMock_CreateDiscussionAsync_Failure()
    //{
    //    var error = Error.Failure("debug.fail", "debugfail");

    //    _discussionModuleContractMock.MockFunction(nameof(IDiscussionModuleContract.CreateDiscussionAsync));
    //    _discussionModuleContractMock.Mock
    //        .CreateDiscussionAsync(
    //            Arg.Any<Guid>(), Arg.Any<IEnumerable<Guid>>()
    //        )
    //        .Returns(error);
    //    return error;
    //}

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
    #endregion
}
