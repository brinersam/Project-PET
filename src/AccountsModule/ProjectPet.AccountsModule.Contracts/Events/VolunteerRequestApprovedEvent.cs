using ProjectPet.SharedKernel.SharedDto;

namespace ProjectPet.AccountsModule.Contracts.Events;
public record VolunteerRequestApprovedEvent(
    Guid userId,
    VolunteerAccountDto accountDto)
{ }
