using ChatBot.CrossCutting.Apm.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace ChatBot.CrossCutting.Apm.Configuration
{
    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder UseApmTracingMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<HttpRequestTracingMiddleware>();
        }
    }
}