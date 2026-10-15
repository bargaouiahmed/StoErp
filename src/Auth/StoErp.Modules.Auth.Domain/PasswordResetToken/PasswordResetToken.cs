

using StoErp.Modules.Auth.Domain.User;

namespace StoErp.Modules.Auth.Domain.PasswordResetToken;

public sealed class PasswordResetToken
{   
    public PasswordResetTokenId Id {get;private set;}
    public PasswordResetTokenHash TokenHash {get;private set;}
    public UserId UserId {get;private set;}
    public User.User? User {get;private set;}
    public DateTimeOffset CreatedAt {get;private set;}
    public DateTimeOffset ExpiresAt {get;private set;}
    public bool IsUsed {get;private set;} = false;
     
    private PasswordResetToken() { }
    private PasswordResetToken(PasswordResetTokenId id, UserId userId,PasswordResetTokenHash tokenHash, DateTimeOffset createdAt, DateTimeOffset expiresAt)
    {
        Id = id;
        TokenHash = tokenHash;
        UserId = userId;
        CreatedAt = createdAt;
        ExpiresAt = expiresAt;
    }
    public static PasswordResetToken Create(PasswordResetTokenId id, UserId userId, PasswordResetTokenHash tokenHash, DateTimeOffset createdAt, DateTimeOffset expiresAt)
    {
        return new PasswordResetToken(id, userId, tokenHash, createdAt, expiresAt);
    }
    public void MarkAsUsed()
    {
        if (IsUsed)
        {
            throw new InvalidOperationException("Password reset token is already used.");
        }
        IsUsed = true;
    }

}
