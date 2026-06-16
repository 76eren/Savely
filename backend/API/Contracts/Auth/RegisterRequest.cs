namespace API.Contracts.Auth;

public sealed record RegisterRequest(
    string UserName,
    string DisplayName,
    string Email,
    string Password);

