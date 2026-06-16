using Application.Auth.DTOs;
using Application.Auth.Interfaces;
using Application.CQRS.Auth.Commands;
using MediatR;

namespace Application.CQRS.Auth.Handlers;

public sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand, AuthResult>
{
    private readonly IAuthService _authService;

    public LogoutCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public Task<AuthResult> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        return _authService.LogoutAsync(request.Dto, cancellationToken);
    }
}
