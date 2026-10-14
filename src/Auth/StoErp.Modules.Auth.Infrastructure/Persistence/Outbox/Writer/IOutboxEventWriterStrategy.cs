
using StoErp.Shared.Application.Events;

namespace StoErp.Modules.Auth.Infrastructure.Persistence.Outbox.Writer;

public interface IOutboxEventWriterStrategy
{
    bool CanHandle(IEvent eventMessage);
    OutboxEntry CreateEntry(IEvent eventMessage);

}
