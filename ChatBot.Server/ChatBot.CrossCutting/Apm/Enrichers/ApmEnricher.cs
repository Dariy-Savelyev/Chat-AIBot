using Elastic.Apm;
using Microsoft.Extensions.Hosting;
using Serilog.Core;
using Serilog.Events;
using static ChatBot.CrossCutting.Apm.Shared.Constants;

namespace ChatBot.CrossCutting.Apm.Enrichers
{
    public class ApmEnricher(IHostEnvironment hostEnvironment) : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            AddTrace(logEvent, propertyFactory);
            AddEnvironment(logEvent, propertyFactory);
        }

        private static void AddTrace(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (Agent.Tracer == null || Agent.Tracer.CurrentTransaction == null ||
                Agent.Tracer.CurrentTransaction.Custom.ContainsKey(TRANSACTION_HAS_ENDED))
            {
                return;
            }

            var property = propertyFactory.CreateProperty(
                TRACE_PROPERTY,
                new
                {
                    id = Agent.Tracer.CurrentTransaction.TraceId
                },
                true);

            logEvent.AddPropertyIfAbsent(property);
        }

        private void AddEnvironment(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var property = propertyFactory.CreateProperty(TRACE_ENVIRONMENT, hostEnvironment.EnvironmentName);

            logEvent.AddPropertyIfAbsent(property);
        }
    }
}