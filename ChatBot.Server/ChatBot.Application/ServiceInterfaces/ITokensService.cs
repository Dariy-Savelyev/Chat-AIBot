using ChatBot.Application.Models.Tokens;
using ChatBot.Domain.Models;

namespace ChatBot.Application.ServiceInterfaces;

public interface ITokensService : IBaseService
{
    Task<string> ValidateAndGetUserIdTokenAsync(string accessToken);
    Task<string> RefreshTokenAsync(string userId);
    Task RevokeTokenAsync(string userId);
}