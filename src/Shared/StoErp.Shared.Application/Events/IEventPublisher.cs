

namespace StoErp.Shared.Application.Events;

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(
        TEvent eventMessage,
        IReadOnlyCollection<string> completedHandlers,
        ISet<string> newlyCompletedHandlers,
        CancellationToken cancellationToken = default) where TEvent : IEvent;
}
