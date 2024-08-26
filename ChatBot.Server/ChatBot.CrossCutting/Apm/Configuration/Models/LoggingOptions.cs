using Serilog.Sinks.Elasticsearch;

namespace ChatBot.CrossCutting.Apm.Configuration.Models
{
    public class LoggingOptions
    {
        public string ServiceName { get; set; }
        public string ServiceInstanceId { get; set; }
        public bool UseElasticsearchLogger { get; set; }
        public bool UseFileLogger { get; set; }
        public Uri ElasticSearchUri { get; set; }
        public ElasticsearchSinkOptions ElasticsearchSinkOptions { get; set; }
        public bool UseDebugLogger { get; set; }
        public bool UseConsoleLogger { get; set; }
    }
}