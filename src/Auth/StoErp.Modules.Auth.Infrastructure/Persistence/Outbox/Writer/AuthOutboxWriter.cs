using StoErp.Modules.Auth.Application.Ports.Outbox;
using StoErp.Shared.Application.Events;

namespace StoErp.Modules.Auth.Infrastructure.Persistence.Outbox.Writer;

public sealed class AuthOutboxWriter : IAuthOutboxWriter
{
    private readonly IAuthOutboxRepository _outboxRepository;
    private readonly IEnumerable<IOutboxEventWriterStrategy> _writerStrategies;
    public AuthOutboxWriter(
        IAuthOutboxRepository outboxRepository,
        IEnumerable<IOutboxEventWriterStrategy> writerStrategies)
    {
        _outboxRepository = outboxRepository;
        _writerStrategies = writerStrategies;
    }
    public void Add(IEvent eventMessage)
    {
        var strategy = _writerStrategies.SingleOrDefault(
            writer => writer.CanHandle(eventMessage));

        if (strategy is null)
        {
            throw new InvalidOperationException(
                $"No outbox writer strategy is registered for event type '{eventMessage.GetType().FullName}'.");
        }

        _outboxRepository.Add(
            strategy.CreateEntry(eventMessage));
    }

}
