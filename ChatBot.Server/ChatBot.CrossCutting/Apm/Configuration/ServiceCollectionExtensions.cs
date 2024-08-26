using ChatBot.CrossCutting.Apm.Configuration.Models;
using ChatBot.CrossCutting.Apm.Enrichers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ChatBot.CrossCutting.Apm.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLogging(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var options = LoggingOptionsBuilder
            .FromConfiguration(builder.Configuration)
            .Build();

        return services.AddLogging(builder, options);
    }

    public static IServiceCollection AddLogging(this IServiceCollection services, WebApplicationBuilder builder, LoggingOptions options)
    {
        services.AddSingleton(options);

        services.AddLogging(logBuilder => logBuilder.AddLogging(builder, options, new ApmEnricher(builder.Environment)));

        return services;
    }
}