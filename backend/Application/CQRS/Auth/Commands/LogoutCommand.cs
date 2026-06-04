using Application.Auth.DTOs;
using MediatR;

namespace Application.CQRS.Auth.Commands;

public sealed record LogoutCommand(RefreshDto Dto) : IRequest<AuthResult>;

