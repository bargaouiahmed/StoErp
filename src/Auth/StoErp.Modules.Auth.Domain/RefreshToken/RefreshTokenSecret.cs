

namespace StoErp.Modules.Auth.Domain.RefreshToken;

public readonly record struct RefreshTokenSecret
{
    public string Value { get; }
    private RefreshTokenSecret(string value)
    {
        Value = value;
    }
    public static RefreshTokenSecret From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("RefreshTokenSecret cannot be empty"
            , nameof(value));
        }
        return new RefreshTokenSecret(value);
    }
    public override string ToString()
    {
        return Value;
    }

}
