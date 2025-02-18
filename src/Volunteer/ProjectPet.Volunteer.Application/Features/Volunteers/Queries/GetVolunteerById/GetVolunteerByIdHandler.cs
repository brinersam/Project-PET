﻿using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.VolunteerModule.Application.Interfaces;
using ProjectPet.VolunteerModule.Contracts.Dto;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteers.Queries.GetVolunteerById;

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