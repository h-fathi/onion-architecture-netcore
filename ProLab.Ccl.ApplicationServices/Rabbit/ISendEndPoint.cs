using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLab.Ccl.ApplicationServices
{
    public interface IMessageQueueEndpoint
    {
        Task Send(string queueName, object message);
        Task Send<T>(T message) where T : IMqRequest;
    }
}
