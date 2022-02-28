using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using ProLab.Infrastructure.Core;
using ProLab.Infrastructure.Core.Behaviors;
using ProLab.Infrastructure.Core.Events;
using ProLab.Infrastructure.Core.ExceptionHandling;
using ProLab.Infrastructure.Web;
using ProLab.Infrastructure.Web.Configuration;
using ProLab.Infrastructure.Web.Logging;
using ProLab.Infrastructure.Web.Logging.Middlewares;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection.Extensions
{
    /// <summary>
    /// Represents extensions of IServiceCollection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register MadiatR and pipelines
        /// </summary>
        /// <param name="services"></param>
        public static void AddCqrs(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            var assembelies = AppDomain.CurrentDomain.GetAssemblies().Where(x=> x.FullName.StartsWith("ProLab")).ToArray();

            services.AddMediatR(assembelies);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

                    
        }
        
        /// <summary>
        /// Register validation behavior and validation rules
        /// </summary>
        /// <param name="services"></param>
        public static void AddFluentValidators(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            var assembelies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.StartsWith("ProLab")).ToArray();

            services.AddValidatorsFromAssemblies(assembelies);

        }

        /// <summary>
        /// Register infrastructure core
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddInfrastructure(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var typeFinder = new AppTypeFinder();
            //create engine and configure service provider
            var engine = EngineContext.Create();
            //register engine
            services.AddSingleton<IEngine>(engine);

            //register type finder
            services.AddSingleton<ITypeFinder>(typeFinder);
            Singleton<ITypeFinder>.Instance = typeFinder;

            //register event publisher
            services.AddSingleton<IEventPublisher, EventPublisher>();

            //register event consumers
            var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
            foreach (var consumer in consumers)
                foreach (var findInterface in consumer.FindInterfaces((type, criteria) =>
                {
                    var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                    return isMatch;
                }, typeof(IConsumer<>)))
                    services.AddScoped(findInterface, consumer);


            //Register auditor service for data audit in persistence
            services.AddScoped<IAuditService, AuditService>();



            //Register domain exception manager
            services.AddSingleton<IDomainExceptionManager, DomainExceptionManager>();

            var exceptionMessages = typeFinder.FindClassesOfType<IDomainExceptionManager>();
            foreach (var exception in exceptionMessages)
                services.AddSingleton(typeof(IDomainExceptionManager), exception);



            services.AddSingleton(services);
        }

       

        /// <summary>
        /// Configure the application HTTP request pipeline
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void ConfigureRequestPipeline(this IApplicationBuilder application)
        {
            EngineContext.Current.ConfigureRequestPipeline(application);
        }



        /// <summary>
        /// Register logging correlation system
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddCorrelationLoggingSystem(this IServiceCollection services)
        {

            IConfiguration configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            var logConfiguration = configuration.GetSection("Logger")?.Get<LoggerConfig>();

            if (logConfiguration != null)
                services.AddSingleton(logConfiguration);


            //register dependencies
            services.AddScoped<ICorrelationIdAccessor, CorrelationIdAccessor>();
            services.AddScoped<LoggingDelegatingHandler>();
        }


        /// <summary>
        /// Register logging correlation middleware
        /// </summary>
        /// <param name="builder">application builder</param>
        public static IApplicationBuilder UseCorrelationLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CorrelationLoggingMiddleware>();
        }
        /// <summary>
        /// Register Request Response logging middleware
        /// </summary>
        /// <param name="builder">application builder</param>
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
        }


 
    }
}