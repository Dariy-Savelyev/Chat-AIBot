using ChatBot.Application.Models;
using FluentValidation;

namespace ChatBot.Application.Validators;

public class ChatCreationValidator : AbstractValidator<ChatModel>
{
    public ChatCreationValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull();
    }
}