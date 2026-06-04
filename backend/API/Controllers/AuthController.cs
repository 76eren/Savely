using System.Security.Claims;
using API.Contracts.Auth;
using Application.Auth.DTOs;
using Application.CQRS.Auth.Commands;
using Application.CQRS.Auth.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public sealed class AuthController : BaseApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AuthController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
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
        RefreshRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new RefreshCommand(new RefreshDto(request.RefreshToken)),
            cancellationToken);

        return MapResult(result);
    }

    [AllowAnonymous]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<AuthResponse>> Logout(
        LogoutRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new LogoutCommand(new RefreshDto(request.RefreshToken)),
            cancellationToken);

        if (!result.Succeeded)
        {
            return MapFailure(result);
        }

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
}
