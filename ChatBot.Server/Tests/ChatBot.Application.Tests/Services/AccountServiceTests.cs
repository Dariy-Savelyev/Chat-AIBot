using AutoMapper;
using ChatBot.Application.ComponentInterfaces;
using ChatBot.Application.Models;
using ChatBot.Application.Services;
using ChatBot.CrossCutting.Constants;
using ChatBot.CrossCutting.Exceptions;
using ChatBot.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace ChatBot.Application.Tests.Services;

public class AccountServiceTests
{
    private readonly Mock<UserManager<User>> _mockUserManager;
    private readonly Mock<SignInManager<User>> _mockSignInManager;
    private readonly Mock<ITokenComponent> _mockTokenComponent;
    private readonly Mock<IMapper> _mockMapper;
    private readonly AccountService _accountService;

    public AccountServiceTests()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        _mockUserManager = new Mock<UserManager<User>>(userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        _mockSignInManager = new Mock<SignInManager<User>>(_mockUserManager.Object, Mock.Of<Microsoft.AspNetCore.Http.IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<User>>(), null!, null!, null!, null!);
        _mockTokenComponent = new Mock<ITokenComponent>();
        _mockMapper = new Mock<IMapper>();

        _accountService = new AccountService(_mockUserManager.Object, _mockSignInManager.Object, _mockTokenComponent.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task RegistrationAsync_ShouldRegisterUser_WhenValidModelProvided()
    {
        // Arrange
        var model = new RegistrationModel { Email = "test@example.com", UserName = "testuser", Password = "Password123!" };
        var user = new User();

        _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null!);
        _mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((User)null!);
        _mockMapper.Setup(x => x.Map<User>(model)).Returns(user);
        _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);
        _mockUserManager.Setup(x => x.AddPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
        _mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
        _mockUserManager.Setup(x => x.UpdateSecurityStampAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);

        // Act
        await _accountService.RegistrationAsync(model);

        // Assert
        _mockUserManager.Verify(x => x.CreateAsync(It.IsAny<User>()), Times.Once);
        _mockUserManager.Verify(x => x.AddPasswordAsync(It.IsAny<User>(), model.Password), Times.Once);
        _mockUserManager.Verify(x => x.AddToRoleAsync(It.IsAny<User>(), UserRoles.User), Times.Once);
        _mockUserManager.Verify(x => x.UpdateSecurityStampAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task RegistrationAsync_ShouldThrowException_WhenUserAlreadyExists()
    {
        // Arrange
        var model = new RegistrationModel { Email = "existing@example.com", UserName = "existinguser", Password = "Password123!" };
        _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new User());

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentValidationException>(() => _accountService.RegistrationAsync(model));
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnToken_WhenValidCredentialsProvided()
    {
        // Arrange
        var model = new LoginModel { Email = "test@example.com", Password = "Password123!" };
        var user = new User();
        var token = "valid_token";

        _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
        _mockSignInManager.Setup(x => x.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(SignInResult.Success);
        _mockTokenComponent.Setup(x => x.RefreshTokenAsync(It.IsAny<User>())).ReturnsAsync(token);

        // Act
        var result = await _accountService.LoginAsync(model);

        // Assert
        Assert.Equal(token, result);
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowException_WhenUserNotFound()
    {
        // Arrange
        var model = new LoginModel { Email = "nonexistent@example.com", Password = "Password123!" };
        _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null!);
        _mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((User)null!);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentValidationException>(() => _accountService.LoginAsync(model));
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowException_WhenAccountIsLockedOut()
    {
        // Arrange
        var model = new LoginModel { Email = "locked@example.com", Password = "Password123!" };
        var user = new User();

        _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
        _mockSignInManager.Setup(x => x.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(SignInResult.LockedOut);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ForbiddenException>(() => _accountService.LoginAsync(model));
        Assert.Contains("Your account has been locked for 20 minutes", exception.Errors.SelectMany(x => x.Messages));
    }
}