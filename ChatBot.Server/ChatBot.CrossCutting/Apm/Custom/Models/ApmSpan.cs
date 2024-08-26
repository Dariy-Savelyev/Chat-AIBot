using Elastic.Apm;
using Elastic.Apm.Api;

namespace ChatBot.CrossCutting.Apm.Custom.Models
{
    public class ApmSpan : IDisposable
    {
        private readonly ISpan _span;

        internal ApmSpan(ISpan span)
        {
            _span = span;
        }

        public string? Body
        {
            get => _span.Context.Db?.Statement;
            set => _span.Context.Db = new Database
            {
                Statement = value,
                Type = "json"
            };
        }

        [Obsolete("IApmTracer should be used instead")]
        public static ApmSpan StartNew(string name, string type)
        {
            var currentTransaction = Agent.Tracer.CurrentTransaction;
            if (currentTransaction != null)
            {
                var span = currentTransaction.StartSpan(name, type);
                return new ApmSpan(span);
            }

            throw new Exception("CurrentTransaction is null");
        }

        public void CaptureException(Exception ex)
        {
            _span.CaptureException(ex);
        }

        public void Dispose()
        {
            _span.End();
            GC.SuppressFinalize(this);
        }
    }
}