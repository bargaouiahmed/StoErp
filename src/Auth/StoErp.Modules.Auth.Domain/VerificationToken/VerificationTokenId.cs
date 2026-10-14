namespace StoErp.Modules.Auth.Domain.VerificationToken;

public readonly record struct VerificationTokenId
{
    public Guid Value { get; }

    private VerificationTokenId(Guid value)
    {
        Value = value;
    }

    public static VerificationTokenId From(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("Verification token Id cannot be empty", nameof(value));
        }
        return new VerificationTokenId(value);
    }
    public static VerificationTokenId New() => new(Guid.CreateVersion7());
    public override string ToString()
    {
        return Value.ToString();
    }

}
