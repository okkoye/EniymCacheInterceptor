using System;
using EniymCacheInterceptor.CacheKeyGenerator;
using EniymCacheInterceptor.CacheProvider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EniymCacheInterceptor
{
    public class EniymCacheInterceptorServiceCollectionExtensions
    {
        public static IServiceCollection AddEniymCacheInterceptor(IServiceCollection services)
        {
            services.TryAddSingleton<IEniymCacheKeyGenerator, DefaultEniymCacheKeyGenerator>();
            services.TryAddSingleton<IEniymCacheProvider, DefaultMemcachedCacheProvider>();

            return services;
        }

        public static IServiceCollection AddEniymCacheInterceptor(IServiceCollection services,
            Action<IServiceCollection> configure)
        {
            configure?.Invoke(services);

            services.TryAddSingleton<IEniymCacheKeyGenerator, DefaultEniymCacheKeyGenerator>();
            services.TryAddSingleton<IEniymCacheProvider, DefaultMemcachedCacheProvider>();

            return services;
        }
    }
}