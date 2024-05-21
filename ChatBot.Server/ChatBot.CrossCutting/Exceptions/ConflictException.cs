using ChatBot.CrossCutting.Models;

namespace ChatBot.CrossCutting.Exceptions;

public sealed class ConflictException : BaseException
{
    public ConflictException(IReadOnlyCollection<ResponseError> errors)
        : base(errors, "Conflict. One or more validation errors occurred") => Errors = errors;
}