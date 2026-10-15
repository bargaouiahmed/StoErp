namespace StoErp.Api.Modules.Auth.Dtos;

public record class RegisterUserRequest(string Username, string Email, string Password,string Firstname, string Lastname, DateTimeOffset DateOfBirth,  string LanguagePreference = "en");
