using ProjectPet.Application.Providers;
using System.Threading.Channels;

namespace ProjectPet.Infrastructure.MessageQueues;
public class FileInfoMessageQueue : IMessageQueue<IEnumerable<FileDataDto>>
{
    private readonly Channel<IEnumerable<FileDataDto>> _channel = Channel.CreateUnbounded<IEnumerable<FileDataDto>>();

    public async Task WriteAsync(IEnumerable<FileDataDto> items, CancellationToken cancellation = default)
    {
        await _channel.Writer.WriteAsync(items, cancellation);
    }

    public async Task<IEnumerable<FileDataDto>> ReadAsync(CancellationToken cancellation = default)
    {
        return await _channel.Reader.ReadAsync(cancellation);
    }
}
