using ChatBot.Application.Models.Tokens;
using ChatBot.Domain.Models;

namespace ChatBot.Application.ComponentInterfaces;

public interface ITokenComponent : IBaseComponent
{
    Task<string> GenerateAccessTokenAsync(User user);
    RefreshToken GenerateRefreshToken(string appUserId);
    Task<RefreshTokenModel> RefreshTokenAsync(User user);
}