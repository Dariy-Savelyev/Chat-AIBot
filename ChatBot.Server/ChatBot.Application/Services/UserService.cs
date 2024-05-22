using AutoMapper;
using ChatBot.Application.Models;
using ChatBot.Application.ServiceInterfaces;
using ChatBot.CrossCutting.Extensions;
using ChatBot.Domain.Models;
using ChatBot.Domain.RepositoryInterfaces;

namespace ChatBot.Application.Services;

public class UserService(IUserRepository userRepository, IMapper mapper) : IUserService
{
    public async Task<bool> RegistrationAsync(RegistrationModel model)
    {
        if (string.IsNullOrEmpty(model.UserName))
        {
            return false;
        }

        if (string.IsNullOrEmpty(model.Email))
        {
            return false;
        }

        if (!model.Password.Equals(model.ConfirmPassword))
        {
            return false;
        }

        if (!EmailValidator.IsValidEmail(model.Email))
        {
            return false;
        }

        if (!await userRepository.IsUniqueEmailAsync(model.Email))
        {
            return false;
        }

        if (!await userRepository.IsUniqueNameAsync(model.UserName))
        {
            return false;
        }

        var user = mapper.Map<User>(model);

        await userRepository.AddAsync(user);

        return true;
    }
}