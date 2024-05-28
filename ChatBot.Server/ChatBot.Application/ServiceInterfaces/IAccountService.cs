using ChatBot.Application.Models;

namespace ChatBot.Application.ServiceInterfaces;

public interface IAccountService : IBaseService
{
    Task<bool> RegistrationAsync(RegistrationModel model);
    Task<bool> LoginAsync(LoginModel model);
}