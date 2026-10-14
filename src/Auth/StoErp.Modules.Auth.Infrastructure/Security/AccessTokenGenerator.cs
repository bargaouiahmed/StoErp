
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using StoErp.Modules.Auth.Application.Commands.Login;
using StoErp.Modules.Auth.Application.Ports.Persistence.Projections;
using StoErp.Modules.Auth.Application.Ports.Security;

namespace StoErp.Modules.Auth.Infrastructure.Security;

public sealed class AccessTokenGenerator : IAccessTokenGenerator
{
    private readonly JwtOptions _jwtOptions;
    public AccessTokenGenerator(JwtOptions jwtOptions)
    {
        _jwtOptions = jwtOptions;
    }
    public AccessTokenResult GenerateAccessToken(UserLoginData userLoginData, TimeSpan timeBeforeExpiration, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var now = DateTimeOffset.UtcNow;
        var expirationTime = now.Add(timeBeforeExpiration);

        Claim[] claims =
        [
            new (
                JwtRegisteredClaimNames.Sub,
                 userLoginData.UserId.Value.ToString()),
            new (
                JwtRegisteredClaimNames.UniqueName,
                 userLoginData.Username.Value),
            new (
                JwtRegisteredClaimNames.Email,
                 userLoginData.Email.Value),
            new (
                JwtRegisteredClaimNames.Jti,
                 Guid.NewGuid().ToString()),

        ];
        var securityKey = new SymmetricSecurityKey(
            Convert.FromBase64String(_jwtOptions.Secret));
        var credentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            notBefore: now.UtcDateTime,
            expires: expirationTime.UtcDateTime,
            signingCredentials: credentials
        );
        return new AccessTokenResult(new JwtSecurityTokenHandler().WriteToken(token), expirationTime);
    }
}
