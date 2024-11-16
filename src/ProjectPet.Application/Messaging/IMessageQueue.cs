namespace ProjectPet.Application.Messaging;

public interface IMessageQueue<T>
{
    Task WriteAsync(T items, CancellationToken cancellation = default);
    Task<T> ReadAsync(CancellationToken cancellation = default);
}