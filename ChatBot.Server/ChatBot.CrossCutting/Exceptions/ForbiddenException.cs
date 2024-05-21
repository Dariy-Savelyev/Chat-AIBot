using ChatBot.CrossCutting.Models;

namespace ChatBot.CrossCutting.Exceptions;

public sealed class ForbiddenException : BaseException
{
    public ForbiddenException(IReadOnlyCollection<ResponseError> errors)
        : base(errors, "Forbidden. One or more validation errors occurred")
        => Errors = errors;
}