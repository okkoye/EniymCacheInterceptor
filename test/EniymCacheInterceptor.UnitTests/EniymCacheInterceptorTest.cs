using System;
using EniymCacheInterceptor.CacheKeyGenerator;
using EniymCacheInterceptor.CacheProvider;
using EniymCacheInterceptor.UnitTests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EniymCacheInterceptor.UnitTests
{
    public class EniymCacheInterceptorTest
    {
        private IEniymCacheProvider _cacheProvider;
        private IEniymCacheKeyGenerator _eniymCacheKeyGenerator;
        private IUserAppService _userApp;

        private User _user = new User()
        {
            Id = 1,
            Name = "test"
        };

        public EniymCacheInterceptorTest()
        {
            var services = new ServiceCollection()
                .AddLogging()
                .AddTransient<IUserAppService, UserAppService>()
                .AddEniymCacheInterceptor(_ =>
                {
                    _.add
                })
                .ConfigureDynamicProxy(config => { config.Interceptors.AddTyped<EniymCacheInterceptor>(); });

            IServiceProvider serviceProvider = services.BuildAspectInjectorProvider();
            _userApp = serviceProvider.GetService<IUserAppService>();
            _eniymCacheKeyGenerator = serviceProvider.GetService<IEniymCacheKeyGenerator>();
            _cacheProvider = serviceProvider.GetService<IEniymCacheProvider>();
        }
    }
}