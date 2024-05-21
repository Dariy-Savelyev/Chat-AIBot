namespace ChatBot.CrossCutting.Models;

public class ResponseError
{
    public ResponseError(IReadOnlyCollection<string> messages)
        : this(string.Empty, messages)
    {
    }

    public ResponseError(string fieldName, IReadOnlyCollection<string> messages)
    {
        FieldName = fieldName;
        Messages = messages;
    }

    public string FieldName { get; }
    public IReadOnlyCollection<string> Messages { get; }
}