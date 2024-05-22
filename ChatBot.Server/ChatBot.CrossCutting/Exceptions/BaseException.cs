using ChatBot.CrossCutting.Models;

namespace ChatBot.CrossCutting.Exceptions;

[Serializable]
public abstract class BaseException : ArgumentException
{
    protected BaseException(IReadOnlyCollection<ResponseError> errors, string message)
        : base(message)
        => Errors = errors;

    public IReadOnlyCollection<ResponseError> Errors { get; protected set; }
}