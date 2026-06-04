namespace API.Contracts.Auth;

public sealed record LoginRequest(string UserHandle, string Password);

