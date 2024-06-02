using AuthApp.Application.Models;
using AuthApp.Domain;
using AutoMapper;

namespace AuthApp.Application.Mapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponse>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id)
            ).ForMember(
                dest => dest.EmailConfirmed,
                opt => opt.MapFrom(src => src.EmailConfirmed)
            ).ForMember(
                dest => dest.Email,
                opt => opt.MapFrom(src => src.Email)
            ).ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Name)
            ).ForMember(
                dest => dest.Document,
                opt => opt.MapFrom(src => src.Document)
            ).ForMember(
                dest => dest.DocumentType,
                opt => opt.MapFrom(src => src.DocumentType)
            ).ForMember(
                dest => dest.HasCompleteRegister,
                opt => opt.MapFrom(src => src.HasCompleteRegister)
            ).ForMember(
                dest => dest.PersonId,
                opt => opt.MapFrom(src => src.PersonId)
            ).ForMember(
                dest => dest.UserType,
                opt => opt.MapFrom(src => src.UserType)
            ).ForMember(
                dest => dest.CreatedAt,
                opt => opt.MapFrom(src => src.CreatedAt)
            ).ForMember(
                dest => dest.EditedAt,
                opt => opt.MapFrom(src => src.EditedAt)
            ).ForMember(
                dest => dest.LockoutEnabled,
                opt => opt.MapFrom(src => src.LockoutEnabled)
            ).ForMember(
                dest => dest.LockoutEnd,
                opt => opt.MapFrom(src => src.LockoutEnd)
            ).ForMember(
                dest => dest.Roles,
                opt => opt.MapFrom(src => src.Roles));
        
        CreateMap<RegisterRequest, User>()
            .ForMember(
                dest => dest.Email,
                opt => opt.MapFrom(src => src.Email)
            ).ForMember(
                dest => dest.UserName,
                opt => opt.MapFrom(src => src.Email)
            ).ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Name)
            ).ForMember(
                dest => dest.Document,
                opt => opt.MapFrom(src => src.Document)
            ).ForMember(
                dest => dest.DocumentType,
                opt => opt.MapFrom(src => src.DocumentType)
            ).ForMember(
                dest => dest.UserType,
                opt => opt.MapFrom(src => src.UserType)
            );
    }
}
