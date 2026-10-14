

namespace StoErp.Shared.Application.Events;

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent eventMessage, CancellationToken cancellationToken = default) where TEvent : IEvent;
}
