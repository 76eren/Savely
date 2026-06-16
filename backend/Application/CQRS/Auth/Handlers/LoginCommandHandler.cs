using Application.Auth.DTOs;
using Application.Auth.Interfaces;
using Application.CQRS.Auth.Commands;
using MediatR;

namespace Application.CQRS.Auth.Handlers;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResult>
{
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public Task<AuthResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return _authService.LoginAsync(request.Dto, cancellationToken);
    }
}
