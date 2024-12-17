namespace ProjectPet.Core.Abstractions;
public interface IDatabaseSeeder
{
    Task SeedAsync(bool verboseLogging = false, CancellationToken cancellationToken = default);
}
