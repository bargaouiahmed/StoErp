

namespace StoErp.Shared.Application.Events;

public interface IEventHandler<in TEvent> where TEvent : IEvent
{

    Task HandleAsync(TEvent eventMessage, CancellationToken cancellationToken = default);
}
