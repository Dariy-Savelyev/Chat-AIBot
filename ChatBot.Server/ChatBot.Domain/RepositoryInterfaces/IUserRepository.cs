using ChatBot.Domain.Models;

namespace ChatBot.Domain.RepositoryInterfaces;

public interface IUserRepository : IBaseRepository<User, int>
{
    Task<bool> IsUniqueEmailAsync(string email);
    Task<bool> IsUniqueNameAsync(string userName);
}