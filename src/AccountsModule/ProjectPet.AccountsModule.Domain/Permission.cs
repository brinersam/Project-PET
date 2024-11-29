namespace ProjectPet.AccountsModule.Domain;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public class Permission
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.