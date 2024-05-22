using ChatBot.Application.Models;

namespace ChatBot.Application.ServiceInterfaces;

public interface ITestService : IBaseService
{
    Task<IEnumerable<TestModel>> GetAllAsync();
}