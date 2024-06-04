using ChatBot.Application.Models.Tokens;
using ChatBot.Domain.Models;

namespace ChatBot.Application.ServiceInterfaces;

public interface ITokensService : IBaseService
{
    Task<User?> ValidateTokenAsync(ValidateTokenModel request);
    Task<RefreshTokenModel> RefreshTokenAsync(User user);
    Task RevokeTokenAsync(string userId);
}