using System;

namespace StoErp.Shared.Contracts.ProfileModuleContracts;

public sealed record Profile
{
    public Guid ProfileId { get; init; }
    public Guid UserId { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public DateTimeOffset DateOfBirth { get; init; }
    public required string LanguagePreference { get; init; } 

}
