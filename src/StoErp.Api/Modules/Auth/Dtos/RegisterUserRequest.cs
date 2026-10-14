namespace StoErp.Api.Modules.Auth.Dtos;

public record class RegisterUserRequest(string Username, string Email, string Password);
