using ChatBot.Domain.Models;

namespace ChatBot.Domain.RepositoryInterfaces;

public interface IUserRepository : IBaseRepository<User, int>
{
    bool IsUniqueEmail(string email);
    bool IsUniqueName(string userName);
}