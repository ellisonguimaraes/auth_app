using AuthApp.Application.Models;
using AuthApp.Domain;
using AutoMapper;

namespace AuthApp.Application.Mapper;

public class TokenProfile : Profile
{
    public TokenProfile()
    {
        CreateMap<Token, TokenResponse>()
            .ForMember(
                dest => dest.AccessToken,
                opt => opt.MapFrom(src => src.AccessToken)
            ).ForMember(
                dest => dest.AccessTokenExpiresIn,
                opt => opt.MapFrom(src => src.AccessTokenExpiresIn)
            ).ForMember(
                dest => dest.RefreshToken,
                opt => opt.MapFrom(src => src.RefreshToken)
            ).ForMember(
                dest => dest.RefreshTokenExpiresIn,
                opt => opt.MapFrom(src => src.RefreshTokenExpiresIn)
            ).ForMember(
                dest => dest.TokenType,
                opt => opt.MapFrom(src => src.TokenType.ToString())
            );
    }
}
