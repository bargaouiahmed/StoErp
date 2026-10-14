namespace StoErp.Modules.Auth.Domain.RefreshToken;

public readonly record struct RefreshTokenId
{
    public Guid Value { get; }
    private RefreshTokenId(Guid value)
    {
        Value = value;
    }
    public static RefreshTokenId New() => new(Guid.CreateVersion7());
    public static RefreshTokenId From(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("Refresh token id cannot be empty"
            , nameof(value));
        }
        return new RefreshTokenId(value);
    }

    public override string ToString()
    {
        return Value.ToString();
    }

}
