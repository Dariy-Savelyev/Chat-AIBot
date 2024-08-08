using AutoMapper;
using ChatBot.Application.Models;
using ChatBot.Application.ServiceInterfaces;
using ChatBot.Domain.Models;
using ChatBot.Domain.RepositoryInterfaces;

namespace ChatBot.Application.Services;

public class ChatService(IChatRepository chatRepository, IMapper mapper) : IChatService
{
    public async Task<int> CreateChatAsync(ChatModel model, string userId)
    {
        var chat = mapper.Map<Chat>(model);
        chat.CreatorId = userId;

        await chatRepository.AddAsync(chat);

        return chat.Id;
    }

    public async Task JoinChatAsync(JoinToChatModel model, string userId)
    {
        await chatRepository.JoinUserAsync(userId, model.ChatId);
    }

    public async Task<IEnumerable<GetAllChatModel>> GetAllChatsAsync()
    {
        var dataBaseChats = await chatRepository.GetAllAsync();

        var chats = mapper.Map<IEnumerable<GetAllChatModel>>(dataBaseChats);

        var listOfChats = new List<GetAllChatModel>();
        listOfChats.AddRange(chats);

        return listOfChats.OrderByDescending(x => x.Id);
    }
}