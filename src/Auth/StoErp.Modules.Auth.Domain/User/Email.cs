

namespace StoErp.Modules.Auth.Domain.User;

public readonly record struct Email
{
    public string Value { get; }
    private Email(string value)
    {
        Value = value;
    }
    public static Email From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Email cannot be empty"
            , nameof(value));
        }
        if (!value.Contains("@"))
        {
            throw new ArgumentException("Email must contain '@' character"
            , nameof(value));
        }
        if (!value.Contains("."))
        {
            throw new ArgumentException("Email must contain '.' character"
            , nameof(value));
        }
        if (value.IndexOf("@") > value.LastIndexOf("."))
        {
            throw new ArgumentException("Email must contain '.' character after '@' character"
            , nameof(value));
        }

        return new Email(value);
    }
    public override string ToString()
    {
        return Value;
    }

}
