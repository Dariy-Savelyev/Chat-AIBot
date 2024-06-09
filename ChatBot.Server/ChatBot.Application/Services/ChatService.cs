using AutoMapper;
using ChatBot.Application.Models;
using ChatBot.Application.ServiceInterfaces;
using ChatBot.Domain.Models;
using ChatBot.Domain.RepositoryInterfaces;

namespace ChatBot.Application.Services;

public class ChatService(IChatRepository chatRepository, IUserRepository userRepository, IMapper mapper) : IChatService
{
    public async Task CreateChatAsync(ChatModel model, string userId)
    {
        var chat = mapper.Map<Chat>(model);
        chat.CreatorId = userId;

        await chatRepository.AddAsync(chat);
    }

    public async Task JoinChatAsync(string userId, int chatId)
    {
        var user = await userRepository.GetByIdAsync(userId);
        var chat = await chatRepository.GetByIdAsync(chatId);

        user!.Chats.Add(chat!);

        await userRepository.SaveChangesAsync();
    }
}