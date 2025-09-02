using Microsoft.AspNetCore.Authentication;

namespace DEVShared.SecretKeyAuthentication;

public class SecretKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    public string HeaderName { get; init; } = "X-Internal-Service-Key";

    public string ExpectedKey { get; set; } = "DEFAULT_SecretKeyAuthentication_KEY";
}
