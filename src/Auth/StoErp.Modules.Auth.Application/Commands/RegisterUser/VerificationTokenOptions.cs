namespace StoErp.Modules.Auth.Application.Commands.RegisterUser;

public sealed class VerificationTokenOptions
{
    public TimeSpan ValidityPeriod { get; init; }
    public int Length { get; init; }
    public bool OnlyNumbers { get; init; }
}