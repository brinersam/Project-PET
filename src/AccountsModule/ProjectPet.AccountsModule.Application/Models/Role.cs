using Microsoft.AspNetCore.Identity;

namespace ProjectPet.AccountsModule.Application.Models;

public class Role : IdentityRole<Guid>
{
    string AccessLevel { get; set; } = string.Empty;
}