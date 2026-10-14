using StoErp.Modules.Auth.Domain.User;

namespace StoErp.Modules.Auth.Application.Ports.Persistence.Projections;

public sealed record UserLoginData(UserId UserId, Username Username, Email Email, PasswordHash PasswordHash, bool Verified, DateTimeOffset CreatedAt, DateTimeOffset? UpdatedAt, DateTimeOffset? DeletedAt);
