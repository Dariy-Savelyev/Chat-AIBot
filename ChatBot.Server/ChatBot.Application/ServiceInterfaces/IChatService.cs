using ChatBot.Application.Models;

namespace ChatBot.Application.ServiceInterfaces;

public interface IChatService : IBaseService
{
    Task CreateChatAsync(ChatModel model, string userId);
}