using Microsoft.AspNetCore.Http;
using ProjectPet.Framework.Authorization;
using ProjectPet.SharedKernel.ErrorClasses;
using System.Security.Claims;

namespace DEVShared.Middlewares;

public class ScopedUserDataMiddleware : IMiddleware
{
    private readonly UserScopedData _userData;

    public ScopedUserDataMiddleware(UserScopedData userData)
    {
        _userData = userData;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        string rawUserId = context.User?.Claims?.FirstOrDefault((c) => c.Type == CustomClaims.ID)?.Value!;
        bool isIdParsed = Guid.TryParse(rawUserId, out Guid userId);

        if (context.User.Identity is null || context.User.Identity.IsAuthenticated == false)
        {
            Error? error = null;
            if (string.IsNullOrWhiteSpace(rawUserId) || isIdParsed == false)
                error = Error.Failure("claim.issue", "UserId is corrupted!");

            _userData.MakeErrored(error);
            await next(context);
            return;
        }

        List<string>? permissions = context
            .User?
            .Claims?
            .Where((c) => c.Type == CustomClaims.PERMISSION && c.Value is not null)
            .Select(x => x.Value)
            .ToList();

        List<string>? roles = context.User?
            .Claims?
            .Where((c) => c.Type == CustomClaims.ROLE && c.Value is not null)
            .Select(x => x.Value)
            .ToList();

        _userData.UserId = userId;
        _userData.Permissions = permissions;
        _userData.Roles = roles;

        await next(context);
    }
}