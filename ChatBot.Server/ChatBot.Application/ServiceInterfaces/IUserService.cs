using ChatBot.Application.Models;

namespace ChatBot.Application.ServiceInterfaces;

public interface IUserService : IBaseService
{
    Task<bool> RegistrationAsync(RegistrationModel model);
}