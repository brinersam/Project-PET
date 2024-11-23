namespace ProjectPet.VolunteerModule.Contracts.Dto;
public record AddressDto(
    string Name,
    string Street,
    string Building,
    string? Block = null!,
    int? Entrance = null!,
    int? Floor = null!,
    int? Apartment = null!);
