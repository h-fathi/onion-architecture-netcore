using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProLab.Infrastructure.Core;
using ProLab.Infrastructure.Data;

namespace ProLab.Ccl.Persistence
{
    /// <summary>
    /// Represents object for the configuring DB context on application startup
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add and configure data layer
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {

            //register data layer
            string connectionString = configuration.GetConnectionString("DefaultDbConnection");
            services.AddDbContext<ApplicationContext>(option => option.UseSqlServer(connectionString));
            services.AddScoped<DbContext, ApplicationContext>();

            //register base repositories
            services.AddBasePersistence(connectionString);

        }

    }
}