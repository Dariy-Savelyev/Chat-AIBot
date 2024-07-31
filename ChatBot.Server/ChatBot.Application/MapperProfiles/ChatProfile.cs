using AutoMapper;
using ChatBot.Application.Models;
using ChatBot.Domain.Models;

namespace ChatBot.Application.MapperProfiles;

public class ChatProfile : Profile
{
    public ChatProfile()
    {
        CreateMap<ChatModel, Chat>()
            .ForMember(
                dest => dest.DateCreate,
                opt => opt.MapFrom(x => DateTime.UtcNow))
            ;

        CreateMap<Chat, GetAllChatModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            ;
    }
}