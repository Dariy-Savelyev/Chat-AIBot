using AutoMapper;
using ChatBot.Application.Models;
using ChatBot.Application.ServiceInterfaces;
using ChatBot.Domain.Models;
using ChatBot.Domain.RepositoryInterfaces;

namespace ChatBot.Application.Services;

public class ChatService(IChatRepository chatRepository, IMapper mapper) : IChatService
{
    public async Task CreateChatAsync(ChatModel model, string userId)
    {
        var chat = mapper.Map<Chat>(model);
        chat.CreatorId = userId;

        await chatRepository.AddAsync(chat);
    }
}