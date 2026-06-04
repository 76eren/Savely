using API.Contracts.Auth;
using Application.Auth.DTOs;
using AutoMapper;

namespace API.Mapping;

public sealed class AuthContractProfile : Profile
{
    public AuthContractProfile()
    {
        CreateMap<AuthTokensDto, AuthResponse>();
        CreateMap<UserDto, UserResponse>();
    }
}

