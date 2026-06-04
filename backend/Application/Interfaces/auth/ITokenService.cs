using Application.Auth.DTOs;
using Domain;

namespace Application.Auth.Interfaces;

public interface ITokenService
{
    AccessTokenDto CreateAccessToken(User user);
    string CreateRefreshToken();
}

