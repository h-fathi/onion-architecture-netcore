﻿using System;
using System.Linq;
using System.Net;
using EasyCaching.Core.Configurations;
using ProLab.Infrastructure.Core.Caching;
using ProLab.Infrastructure.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Extensions.DependencyInjection.Extensions
{
    /// <summary>
    /// Represents extensions of IServiceCollection
    /// </summary>
    public static class CacheServiceCollectionExtensions
    {

        /// <summary>
        /// Register hybrid cache system
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddHybridCachingSystem(this IServiceCollection services)
        {

            IConfiguration configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            var cacheConfiguration = configuration.GetSection("Caching")?.Get<CacheConfig>();
            
            if (cacheConfiguration != null)
                services.AddSingleton(cacheConfiguration);
            else
            {
                throw new ArgumentNullException("please set cache config in appsetting");
            }

            var clusterConfig = cacheConfiguration.Cluster;
            services.AddEasyCaching(option =>
            {
                // local
                option.UseInMemory("Default");
                // distributed
                option.UseRedis(config =>
                {
                    config.DBConfig.Endpoints.Add(new ServerEndPoint(clusterConfig.Host, int.Parse(clusterConfig.Port)));
                    config.DBConfig.Database = 5;
                }, "Redis");

                // combine local and distributed
                option.UseHybrid(config =>
                {
                    config.TopicName = "hybrid-cache-topic";
                    config.EnableLogging = false;

                    // specify the local cache provider name after v0.5.4
                    config.LocalCacheProviderName = "Default";
                    // specify the distributed cache provider name after v0.5.4
                    config.DistributedCacheProviderName = "Redis";
                })
                // use redis bus
                .WithRedisBus(busConf =>
                {
                    busConf.Endpoints.Add(new ServerEndPoint(clusterConfig.Bus.Host, int.Parse(clusterConfig.Bus.Port)));
                });
            });

            //register Cluster Cache Manager
            services.AddSingleton<ICacheManager, HybridCacheManager>();
        }

        /// <summary>
        /// Register memory cache system
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddMemoryCachingSystem(this IServiceCollection services)
        {

            IConfiguration configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            var cacheConfiguration = configuration.GetSection("Caching")?.Get<CacheConfig>();
            if(cacheConfiguration != null)
                services.AddSingleton(cacheConfiguration);
            else
            {
                services.AddSingleton(new CacheConfig());
            }
            services.AddMemoryCache();
            //register Memory Cache Manager
            services.AddSingleton<ICacheManager, MemoryCacheManager>();
        }

        /// <summary>
        /// Register distributed cache system
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddDistributedCachingSystem(this IServiceCollection services)
        {

            IConfiguration configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            var cacheConfiguration = configuration.GetSection("Caching")?.Get<CacheConfig>();
            
            if (cacheConfiguration != null)
                services.AddSingleton(cacheConfiguration);
            else
            {
                throw new ArgumentNullException("please set cache config in appsetting");
            }
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = cacheConfiguration.Distributed.Host + ":" + cacheConfiguration.Distributed.Port;
            });


            //register distributed Cache Manager
            services.AddSingleton<ICacheManager, DistributedCacheManager>();
        }
    }
}