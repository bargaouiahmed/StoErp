using StoErp.Modules.Auth.Domain.User;

namespace StoErp.Modules.Auth.Application.Ports.Persistence.Projections;

public sealed record UserPasswordResetData(
    UserId Id,
    bool IsDeleted,
    bool Verified
);  

