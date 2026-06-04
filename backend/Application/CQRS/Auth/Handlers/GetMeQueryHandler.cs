using Application.Auth.DTOs;
using Application.Auth.Interfaces;
using Application.CQRS.Auth.Queries;
using MediatR;

namespace Application.CQRS.Auth.Handlers;

public sealed class GetMeQueryHandler : IRequestHandler<GetMeQuery, UserDto?>
{
    private readonly IAuthService _authService;

    public GetMeQueryHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public Task<UserDto?> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        return _authService.GetMeAsync(request.UserId, cancellationToken);
    }
}
