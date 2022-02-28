using Microsoft.Extensions.Logging;
using ProLab.Infrastructure.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ProLab.Infrastructure.Core
{
    /// <summary>
    /// Represents the event publisher implementation
    /// </summary>
    public partial class EventPublisher : IEventPublisher
    {
        private readonly ILogger<EventPublisher> _logger;
        public EventPublisher(ILogger<EventPublisher> logger)
        {
            _logger = logger;
        }


        #region Methods

        /// <summary>
        /// Publish event to consumers
        /// </summary>
        /// <typeparam name="TEvent">Type of event</typeparam>
        /// <param name="event">Event object</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task PublishAsync<TEvent>(TEvent @event)
        {
            //get all event consumers
            var consumers = EngineContext.Current.ResolveAll<IConsumer<TEvent>>().ToList();

            foreach (var consumer in consumers)
            {
                try
                {
                    //try to handle published event
                    await consumer.HandleEventAsync(@event);
                }
                catch (Exception exception)
                {
                    //log error, we put in to nested try-catch to prevent possible cyclic (if some error occurs)
                    try
                    {
                        
                        if (_logger == null)
                            return;

                        _logger.LogError(exception.Message, exception);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }

        #endregion
    }
}