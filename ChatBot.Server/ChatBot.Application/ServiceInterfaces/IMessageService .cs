using ChatBot.Application.Models;

namespace ChatBot.Application.ServiceInterfaces;

public interface IMessageService : IBaseService
{
    Task<int?> SendMessageAsync(MessageModel model, string userId);
    Task<IEnumerable<GetAllMessageModel>> GetAllMessagesAsync(int chatId);
    Task SetEmoteAsync(MessageEmoteModel model);
}