namespace StoErp.Modules.Auth.Application.Queries.LoggedInUserDetails;

public record LoggedInUserDetailsQueryResult
{
    public required string Username { get; init; }
    public required string Email {get;init;}
    public required string Firstname {get;init;}
    public required string Lastname {get;init;}
    public required string LanguagePreference {get;init;}
    
}