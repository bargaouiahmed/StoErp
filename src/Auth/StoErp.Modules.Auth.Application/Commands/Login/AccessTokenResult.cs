namespace StoErp.Modules.Auth.Application.Commands.Login;

public record class AccessTokenResult(string AccessToken, DateTimeOffset ExpiresAt);



