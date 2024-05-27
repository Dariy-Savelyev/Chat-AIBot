using ChatBot.Application.Models;
using ChatBot.CrossCutting.Extensions;
using ChatBot.Domain.RepositoryInterfaces;
using FluentValidation;

namespace ChatBot.Application.Validators;

public class RegistrationValidator : AbstractValidator<RegistrationModel>
{
    public RegistrationValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .NotNull()
            .Must(userRepository.IsUniqueName);

        RuleFor(x => x.Email)
            .NotEmpty()
            .NotNull()
            .Must(EmailValidator.IsValidEmail)
            .Must(userRepository.IsUniqueEmail);

        RuleFor(x => x.Password)
            .Equal(x => x.ConfirmPassword);
    }
}