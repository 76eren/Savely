namespace Application.Auth.DTOs;

public sealed record AuthTokensDto(
    string AccessToken,
    DateTime AccessTokenExpiresAt,
    string RefreshToken);

