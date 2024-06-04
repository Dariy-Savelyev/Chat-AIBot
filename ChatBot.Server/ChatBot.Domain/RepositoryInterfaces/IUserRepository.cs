using ChatBot.Domain.Models;

namespace ChatBot.Domain.RepositoryInterfaces;

public interface IUserRepository : IBaseRepository<User, string>
{
    bool IsUniqueEmail(string email);
    bool IsUniqueName(string userName);
    Task<User?> GetUserByEmailAsync(string email);
}