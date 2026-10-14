

namespace StoErp.Modules.Auth.Domain.User;

public readonly record struct PasswordHash

{
    public string Value { get; }
    private PasswordHash(string value)
    {
        Value = value;
    }
    public static PasswordHash From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Password hash cannot be empty"
            , nameof(value));
        }
        return new PasswordHash(value);
    }
    public override string ToString()
    {
        return Value;
    }

}
