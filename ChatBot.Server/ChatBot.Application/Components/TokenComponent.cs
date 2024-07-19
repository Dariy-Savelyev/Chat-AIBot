using ChatBot.Application.ComponentInterfaces;
using ChatBot.Application.Models.Tokens;
using ChatBot.CrossCutting.Constants;
using ChatBot.CrossCutting.Extensions;
using ChatBot.Domain.Models;
using ChatBot.Domain.Repositories;
using ChatBot.Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ChatBot.Application.Components;

public class TokenComponent : ITokenComponent
{
    private readonly SymmetricSecurityKey _key;
    private readonly IConfiguration _config;
    private readonly UserManager<User> _userManager;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly int _jwtTokenExpirationDays;

    public TokenComponent(IConfiguration config, UserManager<User> userManager, IRefreshTokenRepository refreshTokenRepository)
    {
        _config = config;
        _userManager = userManager;
        _refreshTokenRepository = refreshTokenRepository;
        var tokenKey = config.GetValue<string>(ConfigurationConstants.JwtTokenKey);
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey!));
        _jwtTokenExpirationDays = config.GetValue<int>(ConfigurationConstants.JwtTokenExpirationDays);
    }

    public async Task<string> GenerateAccessTokenAsync(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(await GetTokenDescriptorAsync(user));

        return tokenHandler.WriteToken(token);
    }

    public RefreshToken GenerateRefreshToken(string appUserId)
    {
        var randomNumber = new byte[64];
        using (var randomNumberGenerator = RandomNumberGenerator.Create())
        {
            randomNumberGenerator.GetBytes(randomNumber);
        }

        return new RefreshToken
        {
            UserId = appUserId,
            Token = Convert.ToBase64String(randomNumber),
            ExpirationDate = DateTime.UtcNow.AddDays(_jwtTokenExpirationDays)
        };
    }

    public async Task<string> RefreshTokenAsync(User user)
    {
        var refreshToken = await _refreshTokenRepository.GetActiveRefreshTokenAsync(user.Id);

        if (refreshToken == null)
        {
            refreshToken = GenerateRefreshToken(user.Id);
            await _refreshTokenRepository.AddAsync(refreshToken);
        }

        var accessToken = await GenerateAccessTokenAsync(user);

        return accessToken;
    }

    private async Task<SecurityTokenDescriptor> GetTokenDescriptorAsync(User user)
    {
        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(await GetClaimsAsync(user)),
            Expires = DateTime.UtcNow.AddMinutes(_config.GetValue<int>(ConfigurationConstants.JwtTokenValidityInMinutes)),
            SigningCredentials = credentials
        };

        return tokenDescriptor;
    }

    private async Task<IEnumerable<Claim>> GetClaimsAsync(User user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);

        return
        [
            new(JwtRegisteredClaimNames.NameId, user.Id),
            new(ClaimsIdentity.DefaultRoleClaimType, userRoles.First()),
        ];
    }
}