using ChatBot.Domain.Models;
using ChatBot.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Domain.Repositories;

public class MessageRepository(ApplicationContext context) : BaseRepository<Message, int>(context), IMessageRepository;