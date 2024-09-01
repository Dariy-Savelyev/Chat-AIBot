using ChatBot.Application.ComponentInterfaces;
using ChatBot.Application.ServiceInterfaces;
using ChatBot.CrossCutting.Extensions;
using ChatBot.Domain.Models;
using ChatBot.Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ChatBot.Application.Services;

public class TokensService(
    IRefreshTokenRepository refreshTokenRepository,
    UserManager<User> userManager,
    TokenValidationParameters tokenValidationParameters,
    ITokenComponent tokenComponent) : ITokensService
{
    public async Task<string> ValidateAndGetUserIdTokenAsync(string accessToken)
    {
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        string? userId;

        try
        {
            var claims = jwtSecurityTokenHandler.ValidateToken(
                accessToken,
                tokenValidationParameters,
                out var validatedAccessToken);

            userId = claims.GetUserId();
        }
        catch (SecurityTokenExpiredException)
        {
            var jwtToken = jwtSecurityTokenHandler.ReadJwtToken(accessToken);
            userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
        }

        if (userId is null)
        {
            throw ExceptionHelper.GetArgumentException(nameof(accessToken), "Invalid token");
        }

        var refreshToken = await refreshTokenRepository.GetActiveRefreshTokenAsync(userId);
        if (refreshToken is null || refreshToken.ExpirationDate < DateTime.UtcNow)
        {
            throw ExceptionHelper.GetArgumentException(nameof(accessToken), "Invalid or expired refresh token");
        }

        return userId;
    }

    public async Task<string> RefreshTokenAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw ExceptionHelper.GetArgumentException(nameof(userId), "Invalid token");
        }

        return await tokenComponent.RefreshTokenAsync(user);
    }

    public async Task RevokeTokenAsync(string userId)
    {
        await refreshTokenRepository.RevokeAsync(userId);
    }
}