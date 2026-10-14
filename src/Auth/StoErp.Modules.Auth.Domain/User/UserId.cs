

namespace StoErp.Modules.Auth.Domain.User;

public readonly record struct UserId
{

    public Guid Value { get; }
    private UserId(Guid value)
    {
        Value = value;
    }
    public static UserId New() => new(Guid.CreateVersion7());
    public static UserId From(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("User Id cannot be empty"
            , nameof(value));
        }
        return new UserId(value);
    }
    public override string ToString()
    {
        return Value.ToString();
    }
}
