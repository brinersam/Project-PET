using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace DEVShared.HttpFilters;
public class JwtForwardingHttpFilter : IHttpMessageHandlerBuilderFilter
{
    private readonly IServiceProvider _services;

    public JwtForwardingHttpFilter(IServiceProvider services)
    {
        _services = services;
    }

    public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next)
    {
        return builder =>
        {
            next(builder);
            var handler = _services.GetRequiredService<JwtForwardingHttpHandler>();
            builder.AdditionalHandlers.Insert(0, handler); // insert at start of pipeline
        };
    }
}