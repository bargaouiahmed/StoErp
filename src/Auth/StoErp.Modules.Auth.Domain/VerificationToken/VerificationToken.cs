

namespace StoErp.Modules.Auth.Domain.VerificationToken;

public sealed class VerificationToken
{
    public VerificationTokenId Id { get; private set; }
    public User.UserId UserId { get; private set; }
    public VerificationTokenHash TokenHash { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset ExpiresAt { get; private set; }
    public bool IsUsed { get; private set; } = false;
    public User.User? User { get; private set; }
    private VerificationToken() { }
    private VerificationToken(VerificationTokenId id, User.UserId userId, VerificationTokenHash tokenHash, DateTimeOffset createdAt, DateTimeOffset expiresAt)
    {
        Id = id;
        UserId = userId;
        TokenHash = tokenHash;
        CreatedAt = createdAt;
        ExpiresAt = expiresAt;
    }


    public static VerificationToken Create(VerificationTokenId id, User.UserId userId, VerificationTokenHash tokenHash, DateTimeOffset createdAt, DateTimeOffset expiresAt)
    {
        return new VerificationToken(id, userId, tokenHash, createdAt, expiresAt);
    }

    public void MarkAsUsed()
    {
        if (IsUsed)
        {
            throw new InvalidOperationException("Verification token is already used.");
        }
        IsUsed = true;
    }

}
