using CSharpFunctionalExtensions;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.DiscussionsModule.Contracts;
public interface IDiscussionModuleContract
{
    public Task<Result<Guid, Error>> CreateDiscussionAsync(Guid relatedEntityId, IEnumerable<Guid> participantUserIds);
}
