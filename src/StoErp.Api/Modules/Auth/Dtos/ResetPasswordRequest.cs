using System;

namespace StoErp.Api.Modules.Auth.Dtos;

public sealed record ResetPasswordRequest
{
    public required string Email { get; init; }
    public required string NewPassword { get; init; }
    public required string ResetTokenSecret { get; init; }
}
