using ChatBot.Application.Models;

namespace ChatBot.Application.ServiceInterfaces;

public interface ILoginService : IBaseService
{
    Task<bool> LoginAsync(LoginModel model);
}