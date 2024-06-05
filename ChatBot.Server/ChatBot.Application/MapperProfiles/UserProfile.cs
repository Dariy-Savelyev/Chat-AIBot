using AutoMapper;
using ChatBot.Application.Models;
using ChatBot.CrossCutting.Extensions;
using ChatBot.Domain.Models;

namespace ChatBot.Application.MapperProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegistrationModel, User>()
            .ForMember(
                dest => dest.EmailConfirmed,
                opt => opt.MapFrom(s => true))
            ;
    }
}