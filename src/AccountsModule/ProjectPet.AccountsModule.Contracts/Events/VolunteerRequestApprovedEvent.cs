
using ProjectPet.SharedKernel.SharedDto;

namespace DEVShared;
public record VolunteerRequestApprovedEvent(
    Guid userId,
    VolunteerAccountDto accountDto) //todo carry this to NEW contracts just for the originating slice
{ }
