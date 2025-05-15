using CSharpFunctionalExtensions;
using ProjectPet.DiscussionsModule.Application.Interfaces;
using ProjectPet.DiscussionsModule.Contracts;
using ProjectPet.DiscussionsModule.Domain.Models;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.DiscussionsModule.Application;

public class DiscussionModuleContractImplementation : IDiscussionModuleContract
{
    private readonly IDiscussionsRepository _repository;

    public DiscussionModuleContractImplementation(
        IDiscussionsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Guid, Error>> CreateDiscussionAsync(
        Guid relatedEntityId,
        IEnumerable<Guid> participantUserIds)
    {
        var createRes = Discussion.Create(
            relatedEntityId,
            participantUserIds);

        if (createRes.IsFailure)
            return createRes.Error;

        var saveRes = await _repository.Save(createRes.Value);
        if (saveRes.IsFailure)
            return saveRes.Error;

        return createRes.Value.Id;
    }
}
