using ChatBot.Domain.Models;
using ChatBot.Domain.RepositoryInterfaces;

namespace ChatBot.Domain.Repositories;

public class TestRepository(ApplicationContext context) : BaseRepository<Test, int>(context), ITestRepository;