using CSharpFunctionalExtensions;
using ProjectPet.DiscussionsModule.Application;
using ProjectPet.DiscussionsModule.Contracts;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.VolunteerRequests.IntegrationTests.ToggleMocks;
public class DiscussionModuleContractToggleMock : ToggleMockBase<IDiscussionModuleContract, DiscussionModuleContractImplementation>, IDiscussionModuleContract
{
    public Task<Result<Guid, Error>> CreateDiscussionAsync(Guid relatedEntityId, IEnumerable<Guid> participantUserIds)
    {
        if (IsMocked(nameof(CreateDiscussionAsync)))
            return Mock.CreateDiscussionAsync(relatedEntityId, participantUserIds);
        else
            return CreateInstance().CreateDiscussionAsync(relatedEntityId, participantUserIds);
    }
}
