using ChatBot.Application.ComponentInterfaces;
using ChatBot.Application.Models.Tokens;
using ChatBot.Application.ServiceInterfaces;
using ChatBot.CrossCutting.Exceptions;
using ChatBot.CrossCutting.Extensions;
using ChatBot.CrossCutting.Models;
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
    public async Task<User?> ValidateTokenAsync(ValidateTokenModel request)
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

        return await userManager.FindByIdAsync(userId);
    }

    public async Task<RefreshTokenModel> RefreshTokenAsync(User user)
    {
        return await tokenComponent.RefreshTokenAsync(user);
    }

    public async Task RevokeTokenAsync(string userId)
    {
        await refreshTokenRepository.RevokeAsync(userId);
    }
}