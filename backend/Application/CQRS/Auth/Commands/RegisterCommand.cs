using Application.Auth.DTOs;
using MediatR;

namespace Application.CQRS.Auth.Commands;

public sealed record RegisterCommand(RegisterDto Dto) : IRequest<AuthResult>;

