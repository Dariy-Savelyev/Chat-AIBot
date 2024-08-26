using ChatBot.CrossCutting.Apm.Custom.Models;
using Elastic.Apm.Api;

namespace ChatBot.CrossCutting.Apm.Custom
{
    public interface IApmTracer
    {
        ApmTransaction StartTransaction(
            string name,
            string type,
            bool shouldLinkToExisting = true,
            DistributedTracingData? tracingData = null);

        ApmSpan? StartSpan(string name, string type, string? subType = null);
    }
}