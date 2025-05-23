using ProjectPet.VolunteerRequests.Contracts.Dto;

namespace ProjectPet.VolunteerRequests.Contracts.Requests;
public record GetVolunteerRequestFilters(VolunteerRequestStatusDto? Status)
{ };
