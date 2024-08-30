using Elastic.Apm.AspNetCore.DiagnosticListener;
using Elastic.Apm.DiagnosticSource;
using Elastic.Apm.Elasticsearch;
using Elastic.Apm.EntityFrameworkCore;
using Elastic.Apm.Extensions.Hosting;
using Elastic.Apm.GrpcClient;
using Elastic.Apm.MongoDb;
using Elastic.Apm.SqlClient;
using Microsoft.Extensions.Hosting;

namespace ChatBot.CrossCutting.Apm.Configuration;

public static class HostExtensions
{
    public static IHostBuilder ConfigureLogging(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseElasticApm(
            new HttpDiagnosticsSubscriber(),
            new AspNetCoreDiagnosticSubscriber(),
            new EfCoreDiagnosticsSubscriber(),
            new SqlClientDiagnosticSubscriber(),
            new ElasticsearchDiagnosticsSubscriber(),
            new GrpcClientDiagnosticSubscriber(),
            new MongoDbDiagnosticsSubscriber());
    }
}