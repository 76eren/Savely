using Application.Auth.DTOs;
using MediatR;

namespace Application.CQRS.Auth.Commands;

public sealed record RefreshCommand(RefreshDto Dto) : IRequest<AuthResult>;

