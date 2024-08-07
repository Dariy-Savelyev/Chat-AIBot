using AutoMapper;
using ChatBot.Application.Models;
using ChatBot.Domain.Models;

namespace ChatBot.Application.MapperProfiles;

public class MessageProfile : Profile
{
    public MessageProfile()
    {
        CreateMap<MessageModel, Message>()
            .ForMember(
                dest => dest.SendDate,
                opt => opt.MapFrom(x => DateTime.UtcNow))
            ;

        CreateMap<Message, GetAllMessageModel>();
    }
}