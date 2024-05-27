using AutoMapper;
using ChatBot.Application.Models;
using ChatBot.CrossCutting.Extensions;
using ChatBot.Domain.Models;

namespace ChatBot.Application.MapperProfiles;

public class LoginProfile : Profile
{
    public LoginProfile()
    {
        CreateMap<LoginModel, User>()
            .ForMember(
                dest => dest.PasswordHash,
                opt => opt.MapFrom(s => PasswordHasher.HashPassword(s.Password)))
            ;
    }
}