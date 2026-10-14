namespace StoErp.Modules.Auth.Domain.PasswordResetToken;

public record struct  PasswordResetTokenHash
{
    public readonly string Value {get;}
    private PasswordResetTokenHash(string value) => Value = value;

    public static PasswordResetTokenHash From(string value) => new(value);
    public static PasswordResetTokenHash New() => new(Guid.NewGuid().ToString());
    
    public readonly override string ToString() => Value.ToString();

}
