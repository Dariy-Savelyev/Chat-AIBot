using ChatBot.Application.Models;

namespace ChatBot.Application.ServiceInterfaces;

public interface IMessageService : IBaseService
{
    Task<int> SendMessageAsync(HubAddMessageModel model, string userId);
    Task<IEnumerable<GetAllMessageModel>> GetAllMessagesAsync(int chatId);
    Task SetEmoteAsync(MessageEmoteModel model);
}