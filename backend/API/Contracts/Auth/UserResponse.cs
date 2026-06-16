namespace API.Contracts.Auth;

public sealed record UserResponse(
    Guid Id,
    string UserName,
    string DisplayName,
    string Email,
    DateTime CreatedAt,
    DateTime UpdatedAt);

