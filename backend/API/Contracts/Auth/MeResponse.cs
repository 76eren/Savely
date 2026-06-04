namespace API.Contracts.Auth;

public sealed record MeResponse(
    Guid Id,
    string UserHandle,
    string UserName,
    string Email,
    DateTime CreatedAt,
    DateTime UpdatedAt);

