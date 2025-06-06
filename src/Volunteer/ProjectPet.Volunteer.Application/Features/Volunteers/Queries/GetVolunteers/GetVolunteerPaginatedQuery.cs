﻿using ProjectPet.Core.Requests;
using ProjectPet.Core.ResponseModels;
using ProjectPet.VolunteerModule.Contracts.Requests;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteers.Queries.GetVolunteers;

public record GetVolunteerPaginatedQuery
    : PaginatedQueryBase, IMapFromRequest<GetVolunteerPaginatedQuery, GetVolunteerPaginatedRequest>
{
    public static GetVolunteerPaginatedQuery FromRequest(GetVolunteerPaginatedRequest request)
    {
        return new() { Page = request.Page, RecordAmount = request.Take };
    }
}
