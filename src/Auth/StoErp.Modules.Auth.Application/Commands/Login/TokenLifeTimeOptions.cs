namespace StoErp.Modules.Auth.Application.Commands.Login;

public sealed record TokenLifeTimeOptions(TimeSpan AccessTokenLifeTime, TimeSpan RefreshTokenLifeTime);
