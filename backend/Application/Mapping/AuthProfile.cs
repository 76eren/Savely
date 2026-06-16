using Application.Auth.DTOs;
using AutoMapper;
using Domain;

namespace Application.Mapping;

public sealed class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<User, UserDto>();
    }
}

