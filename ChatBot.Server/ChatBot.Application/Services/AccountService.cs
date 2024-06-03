using AutoMapper;
using ChatBot.Application.Models;
using ChatBot.Application.ServiceInterfaces;
using ChatBot.CrossCutting.Extensions;
using ChatBot.Domain.Models;
using ChatBot.Domain.RepositoryInterfaces;

namespace ChatBot.Application.Services;

public class AccountService(IUserRepository userRepository, IMapper mapper) : IAccountService
{
#pragma warning disable SA1401
    public static int UserId;
#pragma warning restore SA1401

    public async Task<bool> RegistrationAsync(RegistrationModel model)
    {
        var user = mapper.Map<User>(model);

        await userRepository.AddAsync(user);

        return true;
    }

    public async Task<bool> LoginAsync(LoginModel model)
    {
        var user = await userRepository.GetUserByEmailAsync(model.Email);

        if (user == null)
        {
            return false;
        }

        var passwordHash = PasswordHasher.HashPassword(model.Password);

        if (passwordHash == user.PasswordHash)
        {
            UserId = user.Id;
        }

        return passwordHash == user.PasswordHash;
    }
}