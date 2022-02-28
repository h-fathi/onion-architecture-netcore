using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProLab.Infrastructure.Core;

namespace ProLab.Infrastructure.Data
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
        public static void AddBasePersistence(this IServiceCollection services, string connectionString)
        {
            //register options
            services.AddDbContext<BaseDbContext>(option => option.UseSqlServer(connectionString));

            //register base repositories
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddScoped(typeof(ICacheableRepository<>), typeof(BaseCacheableRepository<>));
        }

    }
}