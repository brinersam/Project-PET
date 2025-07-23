using Microsoft.AspNetCore.Mvc;
using ProjectPet.Framework;
using ProjectPet.Framework.Authorization;

namespace ProjectPet.Web.DebugControllers;

public class DebugController : CustomControllerBase
{
    [HttpGet("TestUnauthenticated")]
    public async Task<IActionResult> Test(CancellationToken cancellationToken = default)
    {
        return Ok(new string[] { "hii", "hello", "heeee" });
    }

    [HttpGet("Exception")]
    public async Task<IActionResult> Error(CancellationToken cancellationToken = default)
    {
        throw new Exception();
    }

    [Permission(PermissionCodes.PetsRead)]
    [HttpGet("TestAuthenticatedOnly")]
    public async Task<IActionResult> TestAuth(CancellationToken cancellationToken = default)
    {
        return Ok(new string[] { "auth", "succ", "ess" });
    }

    [Permission(PermissionCodes.AdminMasterkey)]
    [HttpGet("TestAdminOnly")]
    public async Task<IActionResult> TestAuthFail(CancellationToken cancellationToken = default)
    {
        return Ok(new string[] { "wont", "see", "this" });
    }
}
