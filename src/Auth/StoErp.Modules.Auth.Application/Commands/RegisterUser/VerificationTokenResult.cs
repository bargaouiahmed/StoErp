using StoErp.Modules.Auth.Domain.VerificationToken;

namespace StoErp.Modules.Auth.Application.Commands.RegisterUser;

public record class VerificationTokenResult(string Secret, DateTimeOffset ExpiresAt);