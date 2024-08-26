using Microsoft.Extensions.Configuration;
using Serilog.Sinks.Elasticsearch;
using static ChatBot.CrossCutting.Apm.Shared.Constants;

namespace ChatBot.CrossCutting.Apm.Configuration.Models;

internal class LoggingOptionsBuilder
{
    private readonly IConfiguration _configuration;
    private readonly LoggingOptions _options = new();

    private Action<ElasticsearchSinkOptions> _elasticConfigurator;

    private LoggingOptionsBuilder(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public static LoggingOptionsBuilder FromConfiguration(IConfiguration configuration)
    {
        return new LoggingOptionsBuilder(configuration);
    }

    public LoggingOptionsBuilder WithConfigurator(Action<ElasticsearchSinkOptions> elasticConfigurator)
    {
        _elasticConfigurator = elasticConfigurator;
        return this;
    }

    public LoggingOptions Build()
    {
        BuildServiceName()
            .BuildServiceInstanceId()
            .BuildLoggersOptions();

        if (_options.UseElasticsearchLogger)
        {
            BuildElasticSearchUri()
                .BuildElasticsearchSinkOptions()
                .BuildModifyConnectionSettings();
        }

        return _options;
    }

    private LoggingOptionsBuilder BuildServiceName()
    {
        var serviceName = _configuration["ServiceName"];
        if (string.IsNullOrEmpty(serviceName))
        {
            throw new InvalidDataException("Service name is not configured in Logging section of application settings");
        }

        _options.ServiceName = serviceName;
        return this;
    }

    private LoggingOptionsBuilder BuildServiceInstanceId()
    {
        var serviceInstanceId = _configuration["Logging:ServiceInstanceId"];
        _options.ServiceInstanceId = serviceInstanceId ?? 1.ToString();

        return this;
    }

    private LoggingOptionsBuilder BuildElasticSearchUri()
    {
        var elasticSearchUri = _configuration["Logging:ElasticSearch:Uri"];
        if (string.IsNullOrEmpty(elasticSearchUri))
        {
            throw new InvalidDataException("ElasticSearch Uri is not configured in Logging section of application settings");
        }

        _options.ElasticSearchUri = new Uri(elasticSearchUri);
        return this;
    }

    private void BuildLoggersOptions()
    {
        var loggers = _configuration.GetSection("Logging:Loggers").Get<string[]>();
        if (loggers != null)
        {
            _options.UseConsoleLogger = loggers.Any(logger => logger == CONSOLE_LOGGER);
            _options.UseDebugLogger = loggers.Any(logger => logger == DEBUG_LOGGER);
            _options.UseElasticsearchLogger = loggers.Any(logger => logger == ELASTICSEARCH_LOGGER);
            _options.UseFileLogger = loggers.Any(logger => logger == FILE_LOGGER);
        }
    }

    private void BuildModifyConnectionSettings()
    {
        var userName = _configuration["ElasticSearch_UserName"];
        var password = _configuration["ElasticSearch_Password"];

        _options.ElasticsearchSinkOptions.ModifyConnectionSettings = conf =>
        {
            conf.ServerCertificateValidationCallback((o, certificate, arg3, arg4) => true);
            return conf.BasicAuthentication(userName, password);
        };
    }

    private LoggingOptionsBuilder BuildElasticsearchSinkOptions()
    {
        var elasticSinkOptions = new ElasticsearchSinkOptions(_options.ElasticSearchUri)
        {
            IndexFormat = $"service-{_options.ServiceName.ToLowerInvariant()}-{{0:yyyy.MM.dd}}",
            InlineFields = true,
            //TypeName = null //for ELK v8
        };

        _configuration.Bind("Logging:ElasticSearch:Options", elasticSinkOptions);

        _elasticConfigurator?.Invoke(elasticSinkOptions);

        _options.ElasticsearchSinkOptions = elasticSinkOptions;
        return this;
    }
}