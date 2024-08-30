namespace ChatBot.CrossCutting.Models;

public class ResponseError(string fieldName, IReadOnlyCollection<string> messages)
{
    public ResponseError(IReadOnlyCollection<string> messages)
        : this(string.Empty, messages)
    {
    }

    public string FieldName { get; } = fieldName;
    public IReadOnlyCollection<string> Messages { get; } = messages;
}