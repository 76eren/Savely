using Application.Auth.DTOs;
using Application.Auth.Interfaces;
using Application.CQRS.Auth.Commands;
using MediatR;

namespace Application.CQRS.Auth.Handlers;

public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResult>
{
    private readonly IAuthService _authService;

    public RegisterCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public Task<AuthResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        return _authService.RegisterAsync(request.Dto, cancellationToken);
    }
}
