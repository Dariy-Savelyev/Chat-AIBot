using Azure.Core;
using ChatBot.Application.ComponentInterfaces;
using ChatBot.Application.Models;
using ChatBot.Application.Models.Tokens;
using ChatBot.Application.ServiceInterfaces;
using ChatBot.CrossCutting.Constants;
using ChatBot.CrossCutting.Extensions;
using ChatBot.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Application.Services;

public class AccountService(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    ITokenComponent tokenComponent) : IAccountService
{
    public async Task RegistrationAsync(RegistrationModel model)
    {
        var email = model.Email.Trim();
        var user = await userManager.FindByEmailAsync(email);
        if (user != null)
        {
            ExceptionHelper.ThrowArgumentException(nameof(model.Email), "The user is already registered with this email.");
        }

        _ = await userManager.CreateAsync(new User { UserName = email, Email = email, EmailConfirmed = true });
        user = await userManager.FindByEmailAsync(email);
        await userManager.AddPasswordAsync(user!, model.Password);
        await userManager.AddToRoleAsync(user!, UserRoles.User);
        await userManager.UpdateSecurityStampAsync(user!);
    }

    public async Task<RefreshTokenModel> LoginAsync(LoginModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Email) ?? await FindByEmailAsync(model.Email);

        var signInResult = await signInManager.CheckPasswordSignInAsync(user!, model.Password, lockoutOnFailure: true);
        if (signInResult.Succeeded)
        {
            return await tokenComponent.RefreshTokenAsync(user!);
        }

        if (signInResult.IsLockedOut)
        {
            throw ExceptionHelper.GetForbiddenException("Your account has been locked for 20 minutes");
        }

        throw ExceptionHelper.GetArgumentException(nameof(LoginModel), "Incorrect credentials");
    }

    private async Task<User?> FindByEmailAsync(string email)
    {
        return await userManager.Users.IgnoreQueryFilters()
            .Where(x => x.NormalizedEmail == userManager.NormalizeEmail(email))
            .SingleOrDefaultAsync();
    }
}