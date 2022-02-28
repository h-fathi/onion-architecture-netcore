using MassTransit;
using ProLab.Infrastructure.Web.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLab.Infrastructure.Web.RabbitMq
{
    public class PublishEndPoint: ISendEndPoint
    {
        private readonly IBus _bus;
        private readonly RabbitMqConfig _config;
        public PublishEndPoint(IBus bus, RabbitMqConfig config)
        {
            _bus = bus;
            _config = config;
        }

        public async Task Send(string queueName, object message)
        {
            var host = _config.Host;
            if (_config.Port > 0)
                host += ":" + _config.Port.ToString();
            if (!string.IsNullOrEmpty(_config.VirtualHost))
                host += _config.VirtualHost.ToString();

            Uri uri = new Uri("rabbitmq://" + host + "/" + queueName);
            var endPoint = await _bus.GetSendEndpoint(uri);

            await endPoint.Send(message);

        }

    }
}
