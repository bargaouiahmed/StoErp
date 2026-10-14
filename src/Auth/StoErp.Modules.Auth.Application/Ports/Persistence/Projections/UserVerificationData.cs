using StoErp.Modules.Auth.Domain.User;

namespace StoErp.Modules.Auth.Application.Ports.Persistence.Projections;

public sealed record UserVerificationData(
    UserId Id,
    bool Verified,
    bool IsDeleted
);
