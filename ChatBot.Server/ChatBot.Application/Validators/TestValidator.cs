using ChatBot.Application.Models;
using ChatBot.Domain.RepositoryInterfaces;
using FluentValidation;

namespace ChatBot.Application.Validators;

public class TestValidator : AbstractValidator<TestModel>
{
    public TestValidator(ITestRepository testRepository)
    {
        RuleFor(x => x.Field)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
            .Must(name => name.Length < testRepository.GetAll().Count())
            .WithMessage("Custom message!");
    }
}