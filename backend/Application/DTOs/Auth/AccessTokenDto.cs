namespace Application.Auth.DTOs;

public sealed record AccessTokenDto(string Token, DateTime ExpiresAt);

