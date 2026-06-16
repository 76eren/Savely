namespace Application.Auth.DTOs;

public sealed record UserDto(
    Guid Id,
    string UserName,
    string DisplayName,
    string Email,
    DateTime CreatedAt,
    DateTime UpdatedAt);

