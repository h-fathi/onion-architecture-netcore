using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProLab.Infrastructure.Web.Configuration;
using RabbitMQ.Client;
using System;

namespace ProLab.Infrastructure.Web.RabbitMq
{
    public static class RabbitMqProvider
    {
        public static void AddRabbitMqPublisher(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));


            IConfiguration configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            var rabbitConfiguration = configuration.GetSection("RabbitMq")?.Get<RabbitMqConfig>();

            if (rabbitConfiguration != null)
                services.AddSingleton(rabbitConfiguration);
            else
            {
                throw new ArgumentNullException(nameof(rabbitConfiguration));
            }



            services.AddMassTransit(mt =>
            {
                mt.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(rabbitConfiguration.Host, rabbitConfiguration.Port, rabbitConfiguration.VirtualHost, h =>
                    {
                        h.Username(rabbitConfiguration.Username);
                        h.Password(rabbitConfiguration.Password);
                    });
                }));
                //mt.UsingRabbitMq((context, cfg) =>
                //{
                //    cfg.Host(rabbitConfiguration.Host, rabbitConfiguration.Port, rabbitConfiguration.VirtualHost, h =>
                //    {
                //        h.Username(rabbitConfiguration.Username);
                //        h.Password(rabbitConfiguration.Password);
                //    });
                //    //cfg.Message<IMqRequest>(e => e.SetEntityName("")); // name of the primary exchange
                //    cfg.Publish<IMqRequest>(e => e.ExchangeType = ExchangeType.Direct); // primary exchange type
                //    cfg.Send<IMqRequest>(e =>
                //    {
                //        e.UseRoutingKeyFormatter(context => context.Message.RouteKey.ToString()); // route by provider 

                //    });
                //});
            });
            services.AddMassTransitHostedService();
            services.AddScoped<ISendEndPoint, PublishEndPoint>();
        }

        public static void AddRabbitMqConsumer(this IServiceCollection services, Action<IRabbitMqBusFactoryConfigurator> consumerRegistrationAction = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));


            IConfiguration configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            var rabbitConfiguration = configuration.GetSection("RabbitMq")?.Get<RabbitMqConfig>();

            if (rabbitConfiguration != null)
                services.AddSingleton(rabbitConfiguration);
            else
            {
                throw new ArgumentNullException(nameof(rabbitConfiguration));
            }
            services.AddMassTransit(mt =>
            {
                mt.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitConfiguration.Host, rabbitConfiguration.Port, rabbitConfiguration.VirtualHost, h =>
                    {
                        h.Username(rabbitConfiguration.Username);
                        h.Password(rabbitConfiguration.Password);
                    });

                    consumerRegistrationAction?.Invoke(cfg);
                });
            });
            services.AddMassTransitHostedService();

        }

    }
}
