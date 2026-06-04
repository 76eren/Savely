using System.Security.Claims;
using API.Contracts.Auth;
using Application.Auth.DTOs;
using Application.Auth.Options;
using Application.CQRS.Auth.Commands;
using Application.CQRS.Auth.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers;

public sealed class AuthController : BaseApiController
{
    private const string AccessTokenCookieName = "access_token";
    private const string RefreshTokenCookieName = "refresh_token";

    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly JwtOptions _jwtOptions;

    public AuthController(IMediator mediator, IMapper mapper, IOptions<JwtOptions> jwtOptions)
    {
        _mediator = mediator;
        _mapper = mapper;
        _jwtOptions = jwtOptions.Value;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthResponse>> Register(
        RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new RegisterCommand(new RegisterDto(request.UserHandle, request.UserName, request.Email, request.Password)),
            cancellationToken);

        return MapResult(result);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthResponse>> Login(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new LoginCommand(new LoginDto(request.UserHandle, request.Password)),
            cancellationToken);

        return MapResult(result);
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthResponse>> Refresh(
        [FromBody] RefreshRequest? request,
        CancellationToken cancellationToken)
    {
        var refreshToken = GetRefreshToken(request);
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return UnauthorizedProblem<AuthResponse>("Invalid refresh token", "Refresh token is missing.");
        }

        var result = await _mediator.Send(
            new RefreshCommand(new RefreshDto(refreshToken)),
            cancellationToken);

        return MapResult(result);
    }

    [AllowAnonymous]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<AuthResponse>> Logout(
        [FromBody] LogoutRequest? request,
        CancellationToken cancellationToken)
    {
        var refreshToken = GetRefreshToken(request is null ? null : new RefreshRequest(request.RefreshToken));
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            ClearAuthCookies();
            return NoContent();
        }

        var result = await _mediator.Send(
            new LogoutCommand(new RefreshDto(refreshToken)),
            cancellationToken);

        if (!result.Succeeded)
        {
            return MapFailure(result);
        }

        ClearAuthCookies();
        return NoContent();
    }

    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserResponse>> Me(CancellationToken cancellationToken)
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdValue, out var userId))
        {
            return UnauthorizedProblem<UserResponse>();
        }

        var me = await _mediator.Send(new GetMeQuery(userId), cancellationToken);
        if (me is null)
        {
            return NotFoundProblem<UserResponse>("User not found", "The current user no longer exists.");
        }

        return Ok(_mapper.Map<UserResponse>(me));
    }

    private ActionResult<AuthResponse> MapResult(AuthResult result)
    {
        if (!result.Succeeded || result.Tokens is null)
        {
            return MapFailure(result);
        }

        SetAuthCookies(result.Tokens);
        var response = _mapper.Map<AuthResponse>(result.Tokens);
        return Ok(response);
    }

    private ActionResult<AuthResponse> MapFailure(AuthResult result)
    {
        return result.ErrorCode switch
        {
            "duplicate_handle" => ConflictProblem<AuthResponse>("User handle already exists", result.ErrorDescription ?? string.Empty),
            "duplicate_email" => ConflictProblem<AuthResponse>("Email already exists", result.ErrorDescription ?? string.Empty),
            "invalid_credentials" => UnauthorizedProblem<AuthResponse>("Invalid credentials", result.ErrorDescription ?? string.Empty),
            "invalid_refresh_token" => UnauthorizedProblem<AuthResponse>("Invalid refresh token", result.ErrorDescription ?? string.Empty),
            _ => BadRequestProblem<AuthResponse>("Authentication failed", result.ErrorDescription ?? string.Empty)
        };
    }

    private void SetAuthCookies(AuthTokensDto tokens)
    {
        if (!string.IsNullOrWhiteSpace(tokens.AccessToken))
        {
            Response.Cookies.Append(
                AccessTokenCookieName,
                tokens.AccessToken,
                BuildCookieOptions(tokens.AccessTokenExpiresAt));
        }

        if (!string.IsNullOrWhiteSpace(tokens.RefreshToken))
        {
            var refreshExpiresAt = DateTimeOffset.UtcNow.AddDays(_jwtOptions.RefreshTokenDays);
            Response.Cookies.Append(
                RefreshTokenCookieName,
                tokens.RefreshToken,
                BuildCookieOptions(refreshExpiresAt));
        }
    }

    private void ClearAuthCookies()
    {
        Response.Cookies.Delete(AccessTokenCookieName, BuildCookieOptions(DateTimeOffset.UtcNow.AddDays(-1)));
        Response.Cookies.Delete(RefreshTokenCookieName, BuildCookieOptions(DateTimeOffset.UtcNow.AddDays(-1)));
    }

    private CookieOptions BuildCookieOptions(DateTimeOffset expiresAt)
    {
        return new CookieOptions
        {
            HttpOnly = true,
            Secure = Request.IsHttps,
            SameSite = SameSiteMode.Lax,
            Expires = expiresAt
        };
    }

    private string? GetRefreshToken(RefreshRequest? request)
    {
        if (!string.IsNullOrWhiteSpace(request?.RefreshToken))
        {
            return request.RefreshToken;
        }

        return Request.Cookies.TryGetValue(RefreshTokenCookieName, out var cookieToken)
            ? cookieToken
            : null;
    }
}
