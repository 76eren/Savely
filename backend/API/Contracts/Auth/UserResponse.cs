namespace API.Contracts.Auth;

public sealed record UserResponse(
    Guid Id,
    string UserHandle,
    string UserName,
    string Email,
    DateTime CreatedAt,
    DateTime UpdatedAt);

