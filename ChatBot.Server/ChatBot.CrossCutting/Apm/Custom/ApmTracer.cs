using ChatBot.CrossCutting.Apm.Custom.Models;
using Elastic.Apm.Api;

namespace ChatBot.CrossCutting.Apm.Custom
{
    public class ApmTracer(ITracer tracer) : IApmTracer
    {
        public ApmTransaction StartTransaction(
            string name,
            string type,
            bool shouldLinkToExisting = true,
            DistributedTracingData? tracingData = null)
        {
            if (shouldLinkToExisting)
            {
                var currentTransaction = tracer?.CurrentTransaction;
                tracingData = currentTransaction?.OutgoingDistributedTracingData;
            }

            var newTransaction = tracer?.StartTransaction(name, type, tracingData, true);

            return new ApmTransaction(newTransaction);
        }

        public ApmSpan? StartSpan(string name, string type, string? subType = null)
        {
            var currentTransaction = tracer?.CurrentTransaction;
            if (currentTransaction != null)
            {
                var span = currentTransaction.StartSpan(name, type, subType);
                return new ApmSpan(span);
            }

            return null;
        }
    }
}