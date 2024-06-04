using ChatBot.Application.Models.Tokens;
using ChatBot.Domain.Models;

namespace ChatBot.Application.ServiceInterfaces;

public interface ITokensService : IBaseService
{
    Task<string> ValidateAndGetUserIdTokenAsync(ValidateTokenModel request);
    Task<RefreshTokenModel> RefreshTokenAsync(string userId);
    Task RevokeTokenAsync(string userId);
}