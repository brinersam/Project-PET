using CSharpFunctionalExtensions;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerRequests.Application.Interfaces;
using ProjectPet.VolunteerRequests.Domain.Models;
using ProjectPet.VolunteerRequests.Infrastructure.Repositories;

namespace ProjectPet.VolunteerRequests.IntegrationTests.ToggleMocks;
public class VolunteerRequestRepositoryToggleMock
    : 
    ToggleMockBase<IVolunteerRequestRepository, VolunteerRequestRepository>,
    IVolunteerRequestRepository
{
    public Task<Result<Guid, Error>> AddAsync(VolunteerRequest volunteer, CancellationToken cancellationToken = default)
    {
        if (IsMocked(nameof(AddAsync)))
            return Mock.AddAsync(volunteer, cancellationToken);
        else
            return CreateInstance().AddAsync(volunteer, cancellationToken);
    }

    public Task<Result<Guid, Error>> Delete(VolunteerRequest volunteer, CancellationToken cancellationToken = default)
    {
        if (IsMocked(nameof(Delete)))
            return Mock.Delete(volunteer, cancellationToken);
        else
            return CreateInstance().Delete(volunteer, cancellationToken);
    }

    public Task<Result<VolunteerRequest, Error>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (IsMocked(nameof(GetByIdAsync)))
            return Mock.GetByIdAsync(id, cancellationToken);
        else
            return CreateInstance().GetByIdAsync(id, cancellationToken);
    }

    public Task<Result<Guid, Error>> Save(VolunteerRequest volunteer, CancellationToken cancellationToken = default)
    {
        if (IsMocked(nameof(Save)))
            return Mock.Save(volunteer, cancellationToken);
        else
            return CreateInstance().Save(volunteer, cancellationToken);
    }
}
