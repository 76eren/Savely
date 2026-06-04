namespace Application.Auth.DTOs;

public sealed record UserDto(
    Guid Id,
    string UserHandle,
    string UserName,
    string Email,
    DateTime CreatedAt,
    DateTime UpdatedAt);

