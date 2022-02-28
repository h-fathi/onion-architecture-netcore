using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using System;
using Serilog.Formatting.Elasticsearch;
using Serilog.Events;
using ProLab.Infrastructure.Web.Configuration;
using Serilog.Sinks.RabbitMQ.Sinks.RabbitMQ;
using Serilog.Sinks.RabbitMQ;

namespace ProLab.Infrastructure.Web.Logging
{
    public static class SeriLogger
    {
        public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
           (context, configuration) =>
           {

               var logConfiguration = context.Configuration.GetSection("Logger")?.Get<LoggerConfig>();
               if (logConfiguration == null)
                   return;

              
               var loggingConfig = configuration
                     .Enrich.FromLogContext()
                        .Enrich.WithMachineName()
                        .Enrich.WithExceptionDetails()
                        .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                        .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName);

               if (logConfiguration.LogProvider.ToLower() == "rabbitmq")
               {
                   var config = new RabbitMQClientConfiguration
                   {
                       Port = logConfiguration.RabbitMqConfig.Port,
                       DeliveryMode = logConfiguration.RabbitMqConfig.Durable.ToRabbitMQDeliveryMode(),
                       Exchange = logConfiguration.RabbitMqConfig.Exchange,
                       Username = logConfiguration.RabbitMqConfig.Username,
                       Password = logConfiguration.RabbitMqConfig.Password,
                       ExchangeType = logConfiguration.RabbitMqConfig.ExchangeType,
                       RouteKey = logConfiguration.RabbitMqConfig.RouteKey
                   };
                   config.Hostnames.Add(logConfiguration.RabbitMqConfig.Host);


                   loggingConfig.WriteTo.RabbitMQ((clientConfiguration, sinkConfiguration) =>
                            {
                                clientConfiguration.From(config);
                                sinkConfiguration.TextFormatter = new ElasticsearchJsonFormatter();
                            });
               }
               else if (logConfiguration.LogProvider.ToLower() == "elastic")
               {
                   var elasticUri = logConfiguration.ElasticConfig.Uri;
                   loggingConfig.WriteTo.Elasticsearch(
                   new ElasticsearchSinkOptions(new Uri(elasticUri))
                   {
                       IndexFormat = $"ProLab-{context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-")}-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                       AutoRegisterTemplate = true,
                       NumberOfShards = 2,
                       NumberOfReplicas = 1
                   });

               }
               else if (logConfiguration.LogProvider.ToLower() == "file")
               {
                   loggingConfig.WriteTo.File(
                         @"C:\Logs\Log.txt",
                         rollingInterval: RollingInterval.Day);
               }

               loggingConfig.ReadFrom.Configuration(context.Configuration);
           };

        public static RabbitMQDeliveryMode ToRabbitMQDeliveryMode(this string delivery)
        {
            switch (delivery)
            {
                case "Durable":
                    return RabbitMQDeliveryMode.Durable;
                case "NonDurable":
                    return RabbitMQDeliveryMode.NonDurable;
                default:
                    return RabbitMQDeliveryMode.NonDurable;
            }
        }
    }
}
