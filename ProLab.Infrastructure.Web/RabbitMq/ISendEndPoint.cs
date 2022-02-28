using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLab.Infrastructure.Web.RabbitMq
{
    public interface ISendEndPoint
    {
        Task Send(string queueName, object message);
    }
}
