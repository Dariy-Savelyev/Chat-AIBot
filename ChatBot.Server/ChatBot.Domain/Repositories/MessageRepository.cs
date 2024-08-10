using ChatBot.Domain.Models;
using ChatBot.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Domain.Repositories;

public class MessageRepository(ApplicationContext context) : BaseRepository<Message, int>(context), IMessageRepository
{
    public async Task AddEmoteAsync(int messageId, bool? emoteValue)
    {
        var message = await Table.Where(x => x.Id == messageId).FirstOrDefaultAsync();

        message!.Emote = emoteValue;

        await SaveChangesAsync();
    }
}