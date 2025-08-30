using Microsoft.AspNetCore.Http;

namespace ProjectPet.AccountsModule.Infrastructure.HttpFilters;
public class JwtForwardingHttpHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _context;

    public JwtForwardingHttpHandler(IHttpContextAccessor context)
    {
        _context = context;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellation)
    {
        if (_context.HttpContext is null)
            return base.SendAsync(request, cancellation);

        if (_context.HttpContext.Request.Headers.TryGetValue("Authorization", out var jwt))
            request.Headers.Add("Authorization", jwt.FirstOrDefault());

        return base.SendAsync(request, cancellation);
    }
}