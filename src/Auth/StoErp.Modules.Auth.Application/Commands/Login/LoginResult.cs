namespace StoErp.Modules.Auth.Application.Commands.Login;

public sealed record LoginResult(string AccessToken, string RefreshToken, DateTimeOffset AccessTokenExpiresAt, DateTimeOffset RefreshTokenExpiresAt);