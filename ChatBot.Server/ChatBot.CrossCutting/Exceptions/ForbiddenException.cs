using ChatBot.CrossCutting.Models;

namespace ChatBot.CrossCutting.Exceptions;

[Serializable]
public sealed class ForbiddenException : BaseException
{
    public ForbiddenException(IReadOnlyCollection<ResponseError> errors)
        : base(errors, "Forbidden. One or more validation errors occurred")
        => Errors = errors;

    public ForbiddenException(string message)
        : base(null!, message)
    {
    }
}