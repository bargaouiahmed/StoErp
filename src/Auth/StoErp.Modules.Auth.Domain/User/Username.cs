

namespace StoErp.Modules.Auth.Domain.User;

public readonly record struct Username

{
    public string Value { get; }
    private Username(string value)
    {
        Value = value;
    }


    public static Username From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Username cannot be empty"
            , nameof(value));
        }
        return new Username(value);
    }
    public override string ToString()
    {
        return Value;
    }

}
