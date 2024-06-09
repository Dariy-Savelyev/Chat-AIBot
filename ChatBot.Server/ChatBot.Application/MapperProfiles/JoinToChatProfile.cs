using AutoMapper;
using ChatBot.Application.Models;
using ChatBot.Application.Services;
using ChatBot.Domain.Models;

namespace ChatBot.Application.MapperProfiles;

public class JoinToChatProfile : Profile
{
    public JoinToChatProfile()
    {
        CreateMap<ChatService, JoinToChatModel>()
            .ForMember(
                dest => dest.ChatId,
                opt => opt.MapFrom(s => s.ChatId))
            ;
    }
}