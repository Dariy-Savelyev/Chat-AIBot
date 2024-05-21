using ChatBot.Domain.Models;
using ChatBot.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Domain.Repositories;

public class UserRepository(ApplicationContext context) : BaseRepository<User, int>(context), IUserRepository
{
    public Task<bool> IsUniqueEmailAsync(string email)
    {
        return Table.AllAsync(x => x.Email != email);
    }

    public Task<bool> IsUniqueNameAsync(string userName)
    {
        return Table.AllAsync(y => y.UserName != userName);
    }
}