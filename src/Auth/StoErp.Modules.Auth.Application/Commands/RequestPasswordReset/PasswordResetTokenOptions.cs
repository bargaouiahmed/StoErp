namespace StoErp.Modules.Auth.Application.Commands.RequestPasswordReset;

public sealed record PasswordResetTokenOptions(
    TimeSpan TokenExpirationTime,
    int TokenLength,
    bool OnlyNumbers
);
