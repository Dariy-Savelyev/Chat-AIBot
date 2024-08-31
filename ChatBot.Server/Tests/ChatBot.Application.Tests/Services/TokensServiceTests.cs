using ChatBot.Application.ComponentInterfaces;
using ChatBot.Application.Services;
using ChatBot.CrossCutting.Exceptions;
using ChatBot.Domain.Models;
using ChatBot.Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatBot.Application.Tests.Services;

public class TokensServiceTests
{
    private readonly Mock<IRefreshTokenRepository> _mockRefreshTokenRepository;
    private readonly Mock<UserManager<User>> _mockUserManager;
    private readonly Mock<ITokenComponent> _mockTokenComponent;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly TokensService _tokensService;

    public TokensServiceTests()
    {
        _mockRefreshTokenRepository = new Mock<IRefreshTokenRepository>();
        _mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null!, null!, null!, null!, null!, null!, null!, null!);
        _mockTokenComponent = new Mock<ITokenComponent>();
        _tokenValidationParameters = new TokenValidationParameters();

        _tokensService = new TokensService(
            _mockRefreshTokenRepository.Object,
            _mockUserManager.Object,
            _tokenValidationParameters,
            _mockTokenComponent.Object);
    }

    private static string GenerateJwtToken(string userId)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var key = new SymmetricSecurityKey("YourTestSecretKeyHereShouldBeAtLeast128bitsLong"u8.ToArray());
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "test",
            audience: "test",
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateExpiredJwtToken(string userId)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.NameId, userId),
        };

        var key = new SymmetricSecurityKey("YourTestSecretKeyHereShouldBeAtLeast128bitsLong"u8.ToArray());
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "test",
            audience: "test",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(-5),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [Fact]
    public async Task ValidateAndGetUserIdTokenAsync_ValidToken_ReturnsUserId()
    {
        // Arrange
        var userId = "testUserId";
        var accessToken = GenerateJwtToken(userId);
        var refreshToken = new RefreshToken { UserId = userId, ExpirationDate = DateTime.UtcNow.AddDays(1) };

        _mockRefreshTokenRepository.Setup(r => r.GetActiveRefreshTokenAsync(userId))
            .ReturnsAsync(refreshToken);

        _tokenValidationParameters.ValidateIssuerSigningKey = true;
        _tokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourTestSecretKeyHereShouldBeAtLeast128bitsLong"));
        _tokenValidationParameters.ValidateIssuer = false;
        _tokenValidationParameters.ValidateAudience = false;
        _tokenValidationParameters.ValidateLifetime = false;

        // Act
        var result = await _tokensService.ValidateAndGetUserIdTokenAsync(accessToken);

        // Assert
        Assert.Equal(userId, result);
    }

    [Fact]
    public async Task ValidateAndGetUserIdTokenAsync_ExpiredToken_ThrowsException()
    {
        // Arrange
        var userId = "testUserId";
        var expiredToken = GenerateExpiredJwtToken(userId);

        _mockRefreshTokenRepository.Setup(r => r.GetActiveRefreshTokenAsync(userId))
            .ReturnsAsync((RefreshToken)null!);

        _tokenValidationParameters.ValidateIssuerSigningKey = true;
        _tokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourTestSecretKeyHereShouldBeAtLeast128bitsLong"));
        _tokenValidationParameters.ValidateIssuer = false;
        _tokenValidationParameters.ValidateAudience = false;
        _tokenValidationParameters.ValidateLifetime = true;
        _tokenValidationParameters.ClockSkew = TimeSpan.Zero;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentValidationException>(() =>
            _tokensService.ValidateAndGetUserIdTokenAsync(expiredToken));
    }

    [Fact]
    public async Task RefreshTokenAsync_ValidUserId_ReturnsNewToken()
    {
        // Arrange
        var userId = "testUserId";
        var user = new User { Id = userId };
        var newToken = "newRefreshToken";

        _mockUserManager.Setup(u => u.FindByIdAsync(userId))
            .ReturnsAsync(user);
        _mockTokenComponent.Setup(t => t.RefreshTokenAsync(user))
            .ReturnsAsync(newToken);

        // Act
        var result = await _tokensService.RefreshTokenAsync(userId);

        // Assert
        Assert.Equal(newToken, result);
    }

    [Fact]
    public async Task RefreshTokenAsync_InvalidUserId_ThrowsException()
    {
        // Arrange
        var userId = "invalidUserId";

        _mockUserManager.Setup(u => u.FindByIdAsync(userId))
            .ReturnsAsync((User)null!);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentValidationException>(() =>
            _tokensService.RefreshTokenAsync(userId));
    }

    [Fact]
    public async Task RevokeTokenAsync_CallsRepository()
    {
        // Arrange
        var userId = "testUserId";

        // Act
        await _tokensService.RevokeTokenAsync(userId);

        // Assert
        _mockRefreshTokenRepository.Verify(r => r.RevokeAsync(userId), Times.Once);
    }
}