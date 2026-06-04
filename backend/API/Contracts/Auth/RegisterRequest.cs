namespace API.Contracts.Auth;

public sealed record RegisterRequest(
    string UserHandle,
    string UserName,
    string Email,
    string Password);

