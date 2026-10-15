namespace StoErp.Modules.Auth.Application.Commands.RequestPasswordReset;

public sealed class PasswordResetTokenOptions{
    public TimeSpan TokenExpirationTime{get;init;}
    public int TokenLength{get;init;}
    public bool OnlyNumbers {get;init;}
}
