namespace ProjectPet.VolunteerModule.Contracts.Requests;
public record DeleteVolunteerRequest(Guid Id, bool SoftDelete = true);
