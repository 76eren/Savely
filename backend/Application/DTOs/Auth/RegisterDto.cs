namespace Application.Auth.DTOs;

public sealed record RegisterDto(
    string UserHandle,
    string UserName,
    string Email,
    string Password);

