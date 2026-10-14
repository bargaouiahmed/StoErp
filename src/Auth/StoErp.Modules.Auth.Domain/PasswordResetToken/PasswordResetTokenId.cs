namespace StoErp.Modules.Auth.Domain.PasswordResetToken;

public record struct PasswordResetTokenId
{
    public readonly Guid Value {get;}
    private PasswordResetTokenId(Guid value) => Value = value;

    public static PasswordResetTokenId From(Guid value) => new(value);
    public static PasswordResetTokenId New() => new(Guid.CreateVersion7());
    
    public readonly override string ToString() => Value.ToString();


}
