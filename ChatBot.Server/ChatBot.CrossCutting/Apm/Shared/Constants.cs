namespace ChatBot.CrossCutting.Apm.Shared;

public class Constants
{
    public const string SERVICE_PROPERTY = "service";
    public const string SERVICE_INSTANCE_PROPERTY = "instance";
    public const string CONTAINER_PROPERTY = "container";
    public const string HOST_PROPERTY = "host";
    public const string CUSTOMER_ID_PROPERTY = "tenant";

    public const string TRACE_ENVIRONMENT = "Environment";
    public const string TRACE_PROPERTY = "trace";
    public const string TRACING_DATA_HEADER = "ApmTracingData";
    public const string CUSTOM_TRANSACTION_NAME_HEADER = "CustomTransactionName";

    public const string DEBUG_LOGGER = "Debug";
    public const string CONSOLE_LOGGER = "Console";
    public const string ELASTICSEARCH_LOGGER = "Elasticsearch";
    public const string FILE_LOGGER = "File";

    public const string EVENT_PROPERTY = "event";

    public const string TRANSACTION_HAS_ENDED = "TransactionHasEnded";
}