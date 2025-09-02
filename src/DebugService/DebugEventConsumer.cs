using DEVShared;
using MassTransit;
using System.Net;

namespace DebugService;

public class DebugEventConsumer : IConsumer<DebugEvent>
{
    private readonly IHttpClientFactory _clientFactory;

    public DebugEventConsumer(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }
    public async Task Consume(ConsumeContext<DebugEvent> context)
    {
        var ct = context.CancellationToken;

        var client = _clientFactory.CreateClient();
        HttpResponseMessage response = await client.GetAsync($"http://projectpet.web:8080/api/Debug/InterServiceEventCallback?data={context.Message.data}", ct);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            string text = await response.Content.ReadAsStringAsync(ct);
            if (string.IsNullOrWhiteSpace(text))
            {
                text = response.StatusCode.ToString();
            }
        }
    }
}
