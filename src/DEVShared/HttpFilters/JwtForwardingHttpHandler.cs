using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace DEVShared.HttpFilters;
public class JwtForwardingHttpHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _context;
    private readonly OptionsTokens _tokenOptions;

    public JwtForwardingHttpHandler(
        IHttpContextAccessor context,
        IOptions<OptionsTokens> optionsTokens)
    {
        _context = context;
        _tokenOptions = optionsTokens.Value;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellation)
    {
        const string key_Secret = "X-Internal-Service-Key";
        const string key_Authorization = "Authorization";

        bool isHttpContextValid = _context.HttpContext is not null;

        if (isHttpContextValid == false)
        {
            request.Headers.Add(key_Secret, _tokenOptions.SecretKey);
            return base.SendAsync(request, cancellation);
        }

        bool isJwtValueValid = _context.HttpContext!.Request.Headers.TryGetValue(key_Authorization, out var jwt) &&
                               string.IsNullOrWhiteSpace(jwt.FirstOrDefault()) == false;

        if (isJwtValueValid)
            request.Headers.Add(key_Authorization, jwt.First());

        return base.SendAsync(request, cancellation);
    }
}