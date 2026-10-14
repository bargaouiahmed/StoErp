
namespace StoErp.Modules.Auth.Domain.User;

public sealed class User
{
    public UserId Id { get; private set; }
    public Username Username { get; private set; }
    public Email Email { get; private set; }
    public PasswordHash PasswordHash { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    public List<RefreshToken.RefreshToken> _refreshTokens = [];
    public List<VerificationToken.VerificationToken> _verificationTokens = [];
    public IReadOnlyCollection<RefreshToken.RefreshToken> RefreshTokens => _refreshTokens.ToList().AsReadOnly();
    public IReadOnlyCollection<VerificationToken.VerificationToken> VerificationTokens => _verificationTokens.ToList().AsReadOnly();
    public bool Verified { get; private set; } = false;

    private User() { }
    private User(UserId id, Username username, Email email, PasswordHash passwordHash, DateTimeOffset createdAt)
    {
        Id = id;
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }
    public static User Create(UserId id, Username username, Email email, PasswordHash passwordHash, DateTimeOffset now)
    {
        return new User(id, username, email, passwordHash, now);
    }
    public void Update(Username username, Email email, PasswordHash passwordHash, DateTimeOffset now)
    {
        if (IsDeleted())
        {
            throw new InvalidOperationException("Cannot update a deleted user.");
        }
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        UpdatedAt = now;
    }
    public void Delete(DateTimeOffset now)
    {
        DeletedAt = now;
    }
    public bool IsDeleted()
    {
        return DeletedAt.HasValue;
    }
    public void MarkVerified()
    {
        if (IsDeleted())
        {
            throw new InvalidOperationException("Cannot verify a deleted user.");
        }
        Verified = true;
    }


}
