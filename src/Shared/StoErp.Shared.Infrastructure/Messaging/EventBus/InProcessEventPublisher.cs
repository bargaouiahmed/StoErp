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
        foreach (var handler in handlers)
        {
            await handler.HandleAsync(eventMessage, cancellationToken);
        }
    }
}
