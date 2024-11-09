namespace ProjectPet.Application.Providers;

public record FileDataDto(
    Stream Stream,
    string ObjectName,
    Guid UserId,
    string Bucket);
