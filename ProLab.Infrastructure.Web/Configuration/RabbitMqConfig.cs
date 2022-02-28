using Serilog.Sinks.RabbitMQ.Sinks.RabbitMQ;

namespace ProLab.Infrastructure.Web.Configuration
{
    public record RabbitMqConfig
    {
        public string Host { get; set; }
        public string VirtualHost { get; set; }
        public ushort Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
       


        public record RabbitQueue
        {
            public string Durable { get; set; }
            public string Exchange { get; set; }
            public string ExchangeType { get; set; }
            public string RouteKey { get; set; }
        }

    }
}
