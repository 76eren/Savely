using Application.Auth.DTOs;
using MediatR;

namespace Application.CQRS.Auth.Queries;

public sealed record GetMeQuery(Guid UserId) : IRequest<UserDto?>;

