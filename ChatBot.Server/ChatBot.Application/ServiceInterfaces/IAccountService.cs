using ChatBot.Application.Models;
using ChatBot.Application.Models.Tokens;
using ChatBot.Domain.Models;

namespace ChatBot.Application.ServiceInterfaces;

public interface IAccountService : IBaseService
{
    Task RegistrationAsync(RegistrationModel model);
    Task<RefreshTokenModel> LoginAsync(LoginModel model);
}