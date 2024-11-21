using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using ProjectPet.SharedKernel.Dto;
using ProjectPet.Core.Abstractions;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteer.Queries.GetVolunteerById;

public class GetVolunteerByIdHandler
{
    private readonly IReadDbContext _readDbContext;

    public GetVolunteerByIdHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Result<VolunteerDto, Error>> HandleAsync(
        GetVolunteerByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _readDbContext.Volunteers
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (volunteer is null)
            return Errors.General.NotFound(typeof(VolunteerDto));

        return volunteer;
    }
}