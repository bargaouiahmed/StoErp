using StoErp.Shared.Application.Events;

namespace StoErp.Modules.Auth.Application.Ports.Outbox;

public interface IAuthOutboxWriter
{
    void Add(IEvent eventMessage);
}
