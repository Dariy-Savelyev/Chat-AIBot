using ChatBot.Domain.Models;
using ChatBot.Domain.RepositoryInterfaces;

namespace ChatBot.Domain.Repositories;

public class ChatRepository(ApplicationContext context) : BaseRepository<Chat, int>(context), IChatRepository;