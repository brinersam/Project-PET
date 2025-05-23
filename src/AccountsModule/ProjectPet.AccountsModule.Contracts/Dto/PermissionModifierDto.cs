namespace ProjectPet.AccountsModule.Contracts.Dto;
public class PermissionModifierDto
{
    public string Code { get; init; } = string.Empty;
    public bool IsAllowed { get; init; }
    public DateTime ExpiresAt { get; init; }
}
