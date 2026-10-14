namespace StoErp.Api.Modules.Auth.Dtos;

public sealed record VerifyUserRequest(
    string Email,
    string TokenSecret
);
