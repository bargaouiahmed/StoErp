
using StoErp.Modules.Auth.Domain.User;

namespace StoErp.Modules.Auth.Domain.RefreshToken;

public sealed class RefreshToken
{
    public RefreshTokenId Id { get; private set; }
    public UserId UserId { get; private set; }
    public RefreshTokenHash Hash { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset ExpiresAt { get; private set; }
    public DateTimeOffset? RevokedAt { get; private set; }
    public User.User? User { get; private set; }

    private RefreshToken() { }
    private RefreshToken(RefreshTokenId id, UserId userId, RefreshTokenHash hash, DateTimeOffset createdAt, DateTimeOffset expiresAt)
    {
        Id = id;
        UserId = userId;
        Hash = hash;
        CreatedAt = createdAt;
        ExpiresAt = expiresAt;
        RevokedAt = null;
    }
    public static RefreshToken Create(RefreshTokenId id, UserId userId, RefreshTokenHash hash, DateTimeOffset createdAt, DateTimeOffset expiresAt)
    {
        return new RefreshToken(id, userId, hash, createdAt, expiresAt);
    }
    public bool IsExpired(DateTimeOffset now)
    {
        return now >= ExpiresAt;
    }
    public bool IsRevoked()
    {
        return RevokedAt.HasValue;
    }
    public bool IsActive(DateTimeOffset now)
    {
        return !IsExpired(now) && !IsRevoked();
    }
    public void Revoke(DateTimeOffset now)
    {
        if (IsRevoked())
        {
            throw new InvalidOperationException("Refresh token is already revoked.");
        }
        if (IsExpired(now))
        {
            throw new InvalidOperationException("Refresh token is expired and cannot be revoked.");
        }
        RevokedAt = now;
    }


}
