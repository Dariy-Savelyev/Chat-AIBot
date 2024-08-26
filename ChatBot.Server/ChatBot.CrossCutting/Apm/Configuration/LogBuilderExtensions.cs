using ChatBot.CrossCutting.Apm.Configuration.Models;
using ChatBot.CrossCutting.Apm.Enrichers;
using ChatBot.CrossCutting.Apm.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using static ChatBot.CrossCutting.Apm.Shared.Constants;

namespace ChatBot.CrossCutting.Apm.Configuration;

public static class LogBuilderExtensions
{
    public static ILoggingBuilder AddLogging(
        this ILoggingBuilder logBuilder,
        WebApplicationBuilder builder,
        LoggingOptions options,
        ApmEnricher? enricher = null,
        Action<LoggerConfiguration>? loggerConfigurator = null)
    {
        logBuilder.AddConfiguration(builder.Configuration.GetSection("LOGGING"));

        logBuilder.ClearProviders();

        if (options.UseDebugLogger)
        {
            logBuilder.AddDebug();
        }

        if (options.UseConsoleLogger)
        {
            logBuilder.AddConsole();
        }

        var serilogLogger = CreateSerilogLogger(builder, enricher, options, loggerConfigurator);

        logBuilder.AddSerilog(serilogLogger);

        return logBuilder;
    }

    public static Serilog.Core.Logger CreateSerilogLogger(
        WebApplicationBuilder builder,
        ApmEnricher? enricher = null,
        LoggingOptions? options = null,
        Action<LoggerConfiguration>? loggerConfigurator = null)
    {
        options ??= LoggingOptionsBuilder
            .FromConfiguration(builder.Configuration)
            .Build();

        var serilogConfiguration = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .MinimumLevel.Override("Elastic.Apm", LogEventLevel.Error)
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.WithProperty(SERVICE_PROPERTY, new { name = options.ServiceName }, true)
            .Enrich.WithProperty(EVENT_PROPERTY, new { dataset = $"{options.ServiceName.ToLowerInvariant()}.log" }, true)
            .Enrich.WithProperty(SERVICE_INSTANCE_PROPERTY, options.ServiceInstanceId, true)

            .Enrich.WithProperty(CONTAINER_PROPERTY, TracingExtensions.GetContainerTraceData(), true)
            .Enrich.WithProperty(HOST_PROPERTY, TracingExtensions.GetHostTraceData(), true)
            .Enrich.FromLogContext();

        if (options.UseElasticsearchLogger)
        {
            serilogConfiguration.WriteTo.Elasticsearch(options.ElasticsearchSinkOptions);
        }

        if (options.UseFileLogger)
        {
            serilogConfiguration.WriteTo.File("logs/log.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 31);
        }

        if (enricher != null)
        {
            serilogConfiguration.Enrich.With(enricher);
        }

        loggerConfigurator?.Invoke(serilogConfiguration);

        var serilogLogger = serilogConfiguration.CreateLogger();

        return serilogLogger;
    }
}