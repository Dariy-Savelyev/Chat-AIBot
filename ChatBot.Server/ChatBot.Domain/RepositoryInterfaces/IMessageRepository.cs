using ChatBot.Domain.Models;

namespace ChatBot.Domain.RepositoryInterfaces;

public interface IMessageRepository : IBaseRepository<Message, int>
{
    Task AddEmoteAsync(int messageId, bool? emoteValue);
}