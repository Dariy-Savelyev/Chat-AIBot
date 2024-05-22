using AutoMapper;
using ChatBot.Application.Models;
using ChatBot.Domain.Models;

namespace ChatBot.Application.MapperProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegistrationModel, User>()
            .ForMember(
                dest => dest.PasswordHash,
                opt => opt.MapFrom(s => s.Password))
            ;
    }
}