using Application.Auth.DTOs;

namespace Application.Auth.Interfaces;

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(RegisterDto dto, CancellationToken cancellationToken = default);
    Task<AuthResult> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default);
    Task<AuthResult> RefreshAsync(RefreshDto dto, CancellationToken cancellationToken = default);
    Task<AuthResult> LogoutAsync(RefreshDto dto, CancellationToken cancellationToken = default);
    Task<UserDto?> GetMeAsync(Guid userId, CancellationToken cancellationToken = default);
}
