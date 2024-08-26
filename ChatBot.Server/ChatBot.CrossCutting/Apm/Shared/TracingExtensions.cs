namespace ChatBot.CrossCutting.Apm.Shared
{
    public static class TracingExtensions
    {
        public static object GetContainerTraceData()
        {
            return new
            {
                id = Environment.MachineName
            };
        }

        public static object GetHostTraceData()
        {
            var containerId = Environment.MachineName;
            var hostName = containerId[..Math.Min(containerId.Length, 12)];

            return new
            {
                name = hostName
            };
        }
    }
}