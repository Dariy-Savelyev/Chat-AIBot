using ChatBot.Domain.Models;

namespace ChatBot.Domain.RepositoryInterfaces;

public interface IChatRepository : IBaseRepository<Chat, int>
{
    Task JoinUserAsync(string userId, int chatId);
}