using AutoMapper;
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
    ITokenComponent tokenComponent,
    IMapper mapper) : IAccountService
{
    public async Task RegistrationAsync(RegistrationModel model)
    {
        var email = model.Email.Trim();
        var userName = model.UserName.Trim();
        var user = await userManager.FindByEmailAsync(email);
        if (user != null)
        {
            ExceptionHelper.ThrowArgumentException(nameof(model.Email), "The user is already registered with this email.");
        }

        user = await userManager.FindByNameAsync(userName);
        if (user != null)
        {
            ExceptionHelper.ThrowArgumentException(nameof(model.Email), "The user is already registered with this UserName.");
        }

        user = mapper.Map<User>(model);

        var identityResult = await userManager.CreateAsync(user);
        if (!identityResult.Succeeded)
        {
            _ = await userManager.DeleteAsync(user!);
            var identityResultErrors = string.Join("; ", identityResult.Errors.Select(x => x.Description));
            ExceptionHelper.ThrowArgumentException(nameof(model.Email), identityResultErrors);
        }

        user = await userManager.FindByEmailAsync(email);
        identityResult = await userManager.AddPasswordAsync(user!, model.Password);
        if (!identityResult.Succeeded)
        {
            _ = await userManager.DeleteAsync(user!);
            var identityResultErrors = string.Join("; ", identityResult.Errors.Select(x => x.Description));
            ExceptionHelper.ThrowArgumentException(nameof(model.Email), identityResultErrors);
        }

        await userManager.AddToRoleAsync(user!, UserRoles.User);
        await userManager.UpdateSecurityStampAsync(user!);
    }

    public async Task<RefreshTokenModel> LoginAsync(LoginModel model)
    {
        var email = model.Email.Trim();
        var user = await userManager.FindByEmailAsync(email) ?? await userManager.FindByNameAsync(email);

        if (user == null)
        {
            throw ExceptionHelper.GetArgumentException(nameof(LoginModel), "Incorrect credentials");
        }

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
}