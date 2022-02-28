using Serilog.Sinks.RabbitMQ.Sinks.RabbitMQ;

namespace ProLab.Infrastructure.Web.Configuration
{
    public record LoggerConfig 
    {
        public string LogProvider { get; set; } = "file";  // rabbit - elastic - file
        public RabbitMq RabbitMqConfig { get; set; }
        public ElasticSearch ElasticConfig { get; set; }


        public record RabbitMq
        {
            public int Port { get; set; }
            public string Durable { get; set; }
            public string Exchange { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string ExchangeType { get; set; }
            public string RouteKey { get; set; }
            public string Host { get; set; }
        }

        public record ElasticSearch
        {
            public string Uri { get; set; }
        }
    }
}
