using Elastic.Apm;
using Microsoft.AspNetCore.Http;
using Serilog.Context;
using static ChatBot.CrossCutting.Apm.Shared.Constants;

namespace ChatBot.CrossCutting.Apm.Http;

public class HttpRequestTracingMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor)
{
    public async Task Invoke(HttpContext context)
    {
            using (LogContext.PushProperty(CUSTOMER_ID_PROPERTY, httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "Unauthorized", true))
            {
                var transaction = Agent.Tracer.CurrentTransaction;
                if (transaction != null)
                {
                    using (LogContext.PushProperty(TRACE_PROPERTY, new { id = transaction.TraceId }, true))
                    {
                        await next(context);
                    }
                }
                else
                {
                    await next(context);
                }
            }
        }
}