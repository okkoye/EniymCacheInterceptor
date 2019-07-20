using System;
using System.Linq;
using System.Reflection;
using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EniymCacheInterceptor
{
    public static class EniymCacheInterceptorExtentions
    {
        public static IServiceProvider AddEniymCacheInterceptor(this IServiceCollection services)
        {
            services.TryAddSingleton<IEniymCacheProvider, DefaultMemcachedCacheProvider>();
            services.TryAddSingleton<IEniymCacheKeyGenerator, DefaultEniymCacheKeyGenerator>();

            bool All(MethodInfo x) =>
                x.CustomAttributes.Any(data => typeof(EniymCacheInterceptorAttribute)
                    .IsAssignableFrom(data.AttributeType));

            services.ConfigureDynamicProxy(config => { config.Interceptors.AddTyped<EniymCacheInterceptor>(All); });

            return services.BuildAspectInjectorProvider();
        }
    }
}