using Application.Auth.DTOs;
using MediatR;

namespace Application.CQRS.Auth.Commands;

public sealed record LoginCommand(LoginDto Dto) : IRequest<AuthResult>;

