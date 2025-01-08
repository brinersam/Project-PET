namespace ProjectPet.Core.Options;
public class OptionsDb
{
    public const string SECTION = "Db";
    public string CStringKey { get; set; } = string.Empty;

    public int SoftDeletedCleanupFrequencyHours { get; set; }
    public int SoftDeletedMaxLifeTimeDays { get; set; }
}
