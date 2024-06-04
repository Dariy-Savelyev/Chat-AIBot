using ChatBot.Domain.Models;
using ChatBot.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Domain.Repositories;

public class UserRepository(ApplicationContext context) : BaseRepository<User, string>(context), IUserRepository
{
    public bool IsUniqueEmail(string email)
    {
        return Table.All(x => x.Email != email);
    }

    public bool IsUniqueName(string userName)
    {
        return Table.All(y => y.UserName != userName);
    }

    public Task<User?> GetUserByEmailAsync(string email)
    {
        return Table.SingleOrDefaultAsync(x => x.Email == email);
    }
}