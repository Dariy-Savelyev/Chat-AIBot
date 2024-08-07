using ChatBot.Application.Models;

namespace ChatBot.Application.ServiceInterfaces;

public interface IMessageService : IBaseService
{
    Task SendMessageAsync(MessageModel model, string userId);
    Task<IEnumerable<GetAllMessageModel>> GetAllMessagesAsync(int chatId);
}