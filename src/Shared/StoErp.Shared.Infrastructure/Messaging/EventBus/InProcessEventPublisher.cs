using StoErp.Shared.Application.Events;

namespace StoErp.Shared.Infrastructure.Messaging.EventBus;

public sealed class InProcessEventPublisher : IEventPublisher
{
    private readonly IServiceProvider _serviceProvider;

    public InProcessEventPublisher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task PublishAsync<TEvent>(
        TEvent eventMessage,
        IReadOnlyCollection<string> completedHandlers,
        ISet<string> newlyCompletedHandlers,
        CancellationToken cancellationToken = default)
        where TEvent : IEvent
    {
        var handlers = _serviceProvider.GetService(
            typeof(IEnumerable<IEventHandler<TEvent>>))
            as IEnumerable<IEventHandler<TEvent>>
            ?? [];
        if (!handlers.Any())
        {
            throw new InvalidOperationException(
                $"No event handlers are registered for event type '{typeof(TEvent).Name}'.");
        }

        var completed = new HashSet<string>(completedHandlers, StringComparer.Ordinal);

        foreach (var handler in handlers)
        {
            var handlerName = handler.GetType().FullName
                ?? throw new InvalidOperationException(
                    $"Could not resolve the name of handler '{handler.GetType().Name}'.");

            if (completed.Contains(handlerName))
            {
                continue;
            }

            await handler.HandleAsync(eventMessage, cancellationToken);

            newlyCompletedHandlers.Add(handlerName);
        }
    }
}
