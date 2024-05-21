using ChatBot.CrossCutting.Models;

namespace ChatBot.CrossCutting.Exceptions;

public sealed class GoneException : BaseException
{
    public GoneException(IReadOnlyCollection<ResponseError> errors)
        : base(errors, "Gone. One or more validation errors occurred")
        => Errors = errors;
}