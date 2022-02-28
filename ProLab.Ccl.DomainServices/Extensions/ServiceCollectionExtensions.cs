using ProLab.Ccl.Domain.ServicesInterface;
using ProLab.Ccl.DomainServices;
using System;
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
        public static void AddDomainServices(this IServiceCollection services)
        {
           
            //register services
            services.AddScoped<IScoreBoardService, ScoreBoardService>();

        }


    }
}