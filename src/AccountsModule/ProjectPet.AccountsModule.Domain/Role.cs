using Microsoft.AspNetCore.Identity;

namespace ProjectPet.AccountsModule.Domain;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public class Role : IdentityRole<Guid>
{
    public List<RolePermission> RolePermissions { get; set; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.