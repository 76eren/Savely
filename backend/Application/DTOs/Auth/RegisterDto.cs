namespace Application.Auth.DTOs;

public sealed record RegisterDto(
    string UserName,
    string DisplayName,
    string Email,
    string Password);

