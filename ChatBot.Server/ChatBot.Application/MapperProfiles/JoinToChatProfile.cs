using AutoMapper;
using ChatBot.Application.Models;
using ChatBot.Domain.Models;

namespace ChatBot.Application.MapperProfiles;

public class JoinToChatProfile : Profile
{
    public JoinToChatProfile()
    {
        CreateMap<Chat, JoinToChatModel>()
            .ForMember(
                dest => dest.ChatId,
                opt => opt.MapFrom(s => s.Id))
            ;
    }
}