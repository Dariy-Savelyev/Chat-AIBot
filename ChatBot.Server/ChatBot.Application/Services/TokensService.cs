using Azure.Core;
using ChatBot.Application.ComponentInterfaces;
using ChatBot.Application.Models.Tokens;
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
    public async Task<string> ValidateAndGetUserIdTokenAsync(ValidateTokenModel request)
    {
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var claims = jwtSecurityTokenHandler.ValidateToken(
            request.AccessToken,
            tokenValidationParameters,
            out var validatedAccessToken);

        var userId = claims.GetUserId();

        if (userId is null)
        {
            throw ExceptionHelper.GetArgumentException(nameof(request), "Invalid token");
        }

        var refreshToken = await refreshTokenRepository.GetActiveRefreshTokenAsync(userId);
        if (validatedAccessToken is not JwtSecurityToken || refreshToken is null)
        {
            throw ExceptionHelper.GetArgumentException(nameof(request), "Invalid token");
        }

        return userId;
    }

    public async Task<RefreshTokenModel> RefreshTokenAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw ExceptionHelper.GetArgumentException(nameof(userId), "Invalid token");
        }

        return await tokenComponent.RefreshTokenAsync(user!);
    }

    public async Task RevokeTokenAsync(string userId)
    {
        await refreshTokenRepository.RevokeAsync(userId);
    }
}