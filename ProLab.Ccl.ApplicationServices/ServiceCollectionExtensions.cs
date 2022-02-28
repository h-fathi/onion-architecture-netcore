using Microsoft.Extensions.Configuration;
using ProLab.Ccl.ApplicationServices;
using ProLab.Ccl.Domain.ServicesInterface;
using ProLab.Ccl.DomainServices;
using ProLab.Ccl.DomainServices.RepositoryInterfaces;
using ProLab.Ccl.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;


namespace Microsoft.Extensions.DependencyInjection.Extensions
{
    /// <summary>
    /// Represents extensions of IServiceCollection
    /// </summary>
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// Register all domain servies
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            
            // register services
            services.AddDomainServices();
            // register data layer
            services.AddPersistence(configuration);


            //register repositories
            services.AddScoped<IScoreBoardRepository, ScoreBoardRepository>();

        }

    }
}