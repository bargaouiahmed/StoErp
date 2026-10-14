namespace StoErp.Modules.Auth.Domain.VerificationToken;

public readonly record struct VerificationTokenHash
{
    public string Value { get; }
    private VerificationTokenHash(string value)
    {
        Value = value;
    }

    public static VerificationTokenHash From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Verification token hash cannot be null or empty", nameof(value));
        }
        return new VerificationTokenHash(value);
    }
    public static VerificationTokenHash New()
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var randomBytes = new byte[32];
        using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }
        var hashBytes = sha256.ComputeHash(randomBytes);
        return new VerificationTokenHash(Convert.ToBase64String(hashBytes));
    }
    public override string ToString()
    {
        return Value;
    }

}
