using Application.Auth.DTOs;
using Application.Auth.Interfaces;
using Application.Auth.Options;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Persistence.Authentication;

public sealed class AuthService : IAuthService
{
    private const string DuplicateUserName = "duplicate_username";
    private const string DuplicateEmail = "duplicate_email";
    private const string InvalidCredentials = "invalid_credentials";
    private const string InvalidRefreshToken = "invalid_refresh_token";
    private const string IdentityError = "identity_error";

    private readonly AppDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly JwtOptions _options;
    private readonly IMapper _mapper;

    public AuthService(
        AppDbContext dbContext,
        UserManager<User> userManager,
        ITokenService tokenService,
        IOptions<JwtOptions> options,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _tokenService = tokenService;
        _options = options.Value;
        _mapper = mapper;
    }

    public async Task<AuthResult> RegisterAsync(RegisterDto dto, CancellationToken cancellationToken = default)
    {
        var normalizedUserName = _userManager.NormalizeName(dto.UserName);
        if (await _userManager.Users.AnyAsync(user => user.NormalizedUserName == normalizedUserName, cancellationToken))
        {
            return AuthResult.Failure(DuplicateUserName, "Username is already taken.");
        }

        if (await _userManager.Users.AnyAsync(user => user.Email == dto.Email, cancellationToken))
        {
            return AuthResult.Failure(DuplicateEmail, "Email is already registered.");
        }

        var user = User.Create(dto.UserName, dto.DisplayName, dto.Email);
        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            var errorMessage = string.Join("; ", result.Errors.Select(error => error.Description));
            return AuthResult.Failure(IdentityError, errorMessage);
        }

        return await IssueTokensAsync(user, cancellationToken);
    }

    public async Task<AuthResult> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default)
    {
        var normalizedUserName = _userManager.NormalizeName(dto.UserName);
        var user = await _userManager.Users.FirstOrDefaultAsync(
            u => u.NormalizedUserName == normalizedUserName,
            cancellationToken);

        if (user is null)
        {
            return AuthResult.Failure(InvalidCredentials, "Invalid username or password.");
        }

        var isValid = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!isValid)
        {
            return AuthResult.Failure(InvalidCredentials, "Invalid username or password.");
        }

        return await IssueTokensAsync(user, cancellationToken);
    }

    public async Task<AuthResult> RefreshAsync(RefreshDto dto, CancellationToken cancellationToken = default)
    {
        var existingToken = await _dbContext.RefreshTokens
            .Include(token => token.User)
            .FirstOrDefaultAsync(token => token.Token == dto.RefreshToken, cancellationToken);

        if (existingToken is null || !existingToken.IsActive)
        {
            return AuthResult.Failure(InvalidRefreshToken, "Refresh token is invalid or expired.");
        }

        var user = existingToken.User;
        existingToken.Revoke();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return await IssueTokensAsync(user, cancellationToken);
    }

    public async Task<AuthResult> LogoutAsync(RefreshDto dto, CancellationToken cancellationToken = default)
    {
        var existingToken = await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(token => token.Token == dto.RefreshToken, cancellationToken);

        if (existingToken is null)
        {
            return AuthResult.Success(new AuthTokensDto(string.Empty, DateTime.UtcNow, string.Empty));
        }

        existingToken.Revoke();
        await _dbContext.SaveChangesAsync(cancellationToken);

        return AuthResult.Success(new AuthTokensDto(string.Empty, DateTime.UtcNow, string.Empty));
    }

    public async Task<UserDto?> GetMeAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
        {
            return null;
        }

        return _mapper.Map<UserDto>(user);
    }

    private async Task<AuthResult> IssueTokensAsync(User user, CancellationToken cancellationToken)
    {
        var accessToken = _tokenService.CreateAccessToken(user);
        var refreshTokenValue = _tokenService.CreateRefreshToken();
        var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(_options.RefreshTokenDays);

        var refreshToken = RefreshToken.Create(refreshTokenValue, refreshTokenExpiresAt, user.Id);
        await _dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var tokens = new AuthTokensDto(accessToken.Token, accessToken.ExpiresAt, refreshTokenValue);
        return AuthResult.Success(tokens);
    }
}
