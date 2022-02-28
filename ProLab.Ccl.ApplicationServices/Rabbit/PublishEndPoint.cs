using MassTransit;
using ProLab.Infrastructure.Web.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLab.Ccl.ApplicationServices
{
    public class MessageQueueEndpoint: IMessageQueueEndpoint
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IBus _bus;
        private readonly RabbitMqConfig _config;
        public MessageQueueEndpoint(ISendEndpointProvider sendEndpointProvider, IBus bus, RabbitMqConfig config)
        {
            _sendEndpointProvider = sendEndpointProvider;
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

            Uri uri = new Uri("rabbitmq://" + host+ "/" + queueName);
            //Uri uri = new Uri("queue:" + queueName);
            var endpoint = await _bus.GetSendEndpoint(uri);
            await endpoint.Send(message);

        }
        public async Task Send<T>(T message) where T : IMqRequest
        {
            //var host = _config.Host;
            //if (_config.Port > 0)
            //    host += ":" + _config.Port.ToString();
            //if (!string.IsNullOrEmpty(_config.VirtualHost))
            //    host += _config.VirtualHost.ToString();

            Uri uri = new Uri("queue:" + message.RouteKey);
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(uri);
            await endpoint.Send(message);

        }
    }
}
