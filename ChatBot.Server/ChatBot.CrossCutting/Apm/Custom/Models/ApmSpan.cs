using Elastic.Apm.Api;

namespace ChatBot.CrossCutting.Apm.Custom.Models;

public class ApmSpan : IDisposable
{
    private readonly ISpan _span;
    private bool _disposed;

    internal ApmSpan(ISpan span)
    {
        _span = span;
    }

    ~ApmSpan() => Dispose(false);

    public string? Body
    {
        get => _span.Context.Db?.Statement;
        set => _span.Context.Db = new Database
        {
            Statement = value,
            Type = "json"
        };
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _span.End();
            }

            _disposed = true;
        }
    }
}