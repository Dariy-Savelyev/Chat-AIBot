using Elastic.Apm.Api;
using static ChatBot.CrossCutting.Apm.Shared.Constants;

namespace ChatBot.CrossCutting.Apm.Custom.Models;

public class ApmTransaction : IDisposable
{
    private readonly ITransaction? _transaction;
    private bool _disposed;

    internal ApmTransaction(ITransaction? transaction)
    {
        _transaction = transaction;
    }

    ~ApmTransaction() => Dispose(false);

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
                _transaction?.End();
                _transaction?.Custom.Add(TRANSACTION_HAS_ENDED, string.Empty);
            }

            _disposed = true;
        }
    }
}