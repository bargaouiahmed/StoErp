

namespace StoErp.Modules.Auth.Domain.RefreshToken;

public readonly record struct RefreshTokenHash
{
    public string Value { get; }
    private RefreshTokenHash(string value)
    {
        Value = value;
    }
    public static RefreshTokenHash From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("RefreshTokenHash cannot be empty"
            , nameof(value));
        }
        return new RefreshTokenHash(value);
    }
    public override string ToString()
    {
        return Value;
    }

}
