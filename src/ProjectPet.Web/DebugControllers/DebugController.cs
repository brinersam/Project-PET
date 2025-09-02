using DEVShared;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ProjectPet.AccountsModule.Domain;
using ProjectPet.FileService.Contracts;
using ProjectPet.Framework;
using ProjectPet.Framework.Authorization;
using System.Net;
using Err = ProjectPet.SharedKernel.ErrorClasses;

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

    [Permission(PermissionCodes.AdminMasterkey)]
    [HttpGet("JwtForwarding")]
    public async Task<IActionResult> JwtForwarding(
        [FromServices] IFileService fileService,
        [FromServices] IHttpClientFactory clientFactory,
        CancellationToken ct = default)
    {
        var client = clientFactory.CreateClient();
        HttpResponseMessage response = await client.GetAsync("http://projectpet.debugservice:3080/debugep", ct);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            string text = await response.Content.ReadAsStringAsync(ct);
            if (string.IsNullOrWhiteSpace(text))
            {
                text = response.StatusCode.ToString();
            }

            return Err.Error.Failure("fileservice.error", text).ToResponse();
        }
        var responseData = await response.Content.ReadAsStringAsync(ct);

        return Ok(responseData);
    }

    [HttpGet("InterServiceEvent")]
    public async Task<IActionResult> SendDebugEvent(
        [FromServices] IPublishEndpoint publish,
        [FromServices] IMemoryCache memoryCache,
        CancellationToken cancellationToken = default)
    {
        publish.Publish(new DebugEvent("event"));
        await Task.Delay(2000);

        memoryCache.TryGetValue("DebugValue", out string memoryValue);

        return Ok(new string[] { "interservice event sent! Result: ", memoryValue ?? "no value was set"});
    }

    [Permission(PermissionCodes.AdminMasterkey)]
    [HttpGet("InterServiceEventCallback")]
    public async Task<IActionResult> DebugEventCallback(
        [FromServices] IPublishEndpoint publish,
        [FromServices] IMemoryCache memoryCache,
        [FromQuery] string data,
        CancellationToken cancellationToken = default)
    {
        memoryCache.Set("DebugValue", DateTime.Now.ToString() + " :: " + data);
        return Ok("Request received!");
    }
}
