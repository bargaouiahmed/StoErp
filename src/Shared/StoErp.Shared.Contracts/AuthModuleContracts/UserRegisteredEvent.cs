
using StoErp.Shared.Application.Events;

namespace StoErp.Shared.Contracts.AuthModuleContracts;

public sealed record UserRegisteredEvent(Guid UserId, string Username, string Email, DateTimeOffset RegisteredAt) : IEvent;

