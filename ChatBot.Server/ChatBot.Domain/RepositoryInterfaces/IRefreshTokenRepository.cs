using ChatBot.Domain.Models;
using System.Collections;

namespace ChatBot.Domain.RepositoryInterfaces;

public interface IRefreshTokenRepository : IBaseRepository<RefreshToken, int>
{
    Task<RefreshToken?> GetActiveRefreshTokenAsync(string userId);
    Task RevokeAsync(string userId);
}