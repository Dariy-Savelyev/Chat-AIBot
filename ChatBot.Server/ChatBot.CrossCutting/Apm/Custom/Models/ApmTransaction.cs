using Elastic.Apm;
using Elastic.Apm.Api;
using static ChatBot.CrossCutting.Apm.Shared.Constants;

namespace ChatBot.CrossCutting.Apm.Custom.Models
{
    public class ApmTransaction : IDisposable
    {
        private readonly ITransaction? _transaction;

        internal ApmTransaction(ITransaction? transaction)
        {
            _transaction = transaction;
        }

        public string TraceId => _transaction?.TraceId ?? string.Empty;

        [Obsolete("IApmTracer should be used instead")]
        public static ApmTransaction StartNew(string name, string type, bool shouldLinkToExisting = true, DistributedTracingData? tracingData = null)
        {
            if (shouldLinkToExisting)
            {
                var currentTransaction = Agent.Tracer.CurrentTransaction;
                tracingData = currentTransaction?.OutgoingDistributedTracingData;
            }

            var newTransaction = Agent.Tracer.StartTransaction(name, type, tracingData);

            return new ApmTransaction(newTransaction);
        }

        public void Dispose()
        {
            _transaction?.End();
            _transaction?.Custom.Add(TRANSACTION_HAS_ENDED, string.Empty);
            GC.SuppressFinalize(this);
        }
    }
}