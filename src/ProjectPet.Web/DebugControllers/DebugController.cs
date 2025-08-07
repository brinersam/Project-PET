using Microsoft.AspNetCore.Mvc;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.Framework;
using ProjectPet.Framework.Authorization;
using tempShared.Framework.Authorization;

namespace ProjectPet.Web.DebugControllers;

public class DebugController : CustomControllerBase
{
    [HttpGet("TestUnauthenticated")]
    public async Task<IActionResult> Test(CancellationToken cancellationToken = default)
    {
        await Task.Delay(2000, cancellationToken);
        return Ok(new string[] { "hii", "hello", "heeee" });
    }

    [HttpGet("Exception")]
    public async Task<IActionResult> Error(CancellationToken cancellationToken = default)
    {
        await Task.Delay(2000, cancellationToken);
        throw new Exception();
    }

    [Permission(PermissionCodes.PetsRead)]
    [HttpGet("TestAuthenticatedOnly")]
    public async Task<IActionResult> TestAuth(
        [FromServices] UserScopedData userData,
        CancellationToken cancellationToken = default)
    {
        if (userData.IsSuccess == false)
            return userData.Error!.ToResponse();

        await Task.Delay(2000, cancellationToken);
        return Ok(new string[] { "auth", "succ", "ess", userData.UserId!.ToString() });
    }

    [Permission(PermissionCodes.AdminMasterkey)]
    [HttpGet("TestAdminOnly")]
    public async Task<IActionResult> TestAuthFail(CancellationToken cancellationToken = default)
    {
        await Task.Delay(2000, cancellationToken);
        return Ok(new string[] { "wont", "see", "this" });
    }
}
