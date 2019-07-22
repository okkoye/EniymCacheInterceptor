using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using BenchmarkDotNet.Attributes;
using EniymCacheInterceptor.CacheKeyGenerator;
using EniymCacheInterceptor.CacheProvider;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EniymCacheInterceptor.Benchmark
{
    [AllStatisticsColumn]
    [MemoryDiagnoser]
    [InProcess]
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
                   .AddMemoryCache()
                   .AddLogging()
                   .AddTransient<IUserAppService, UserAppService>()
                   .AddTransient<IEniymCacheProvider, DefaultMemoryCacheProvider>()
                   .AddTransient<IEniymCacheKeyGenerator, DefaultEniymCacheKeyGenerator>()
                   .ConfigureDynamicProxy(config => { config.Interceptors.AddTyped<EniymCacheInterceptor>(); });

            IServiceProvider serviceProvider = services.BuildAspectInjectorProvider();
            _userApp = serviceProvider.GetService<IUserAppService>();
            _eniymCacheKeyGenerator = serviceProvider.GetService<IEniymCacheKeyGenerator>();
            _cacheProvider = serviceProvider.GetService<IEniymCacheProvider>();
        }

        [Benchmark]
        public async Task GetUserName_UsuallyAsync()
        {
            await _userApp.GetUserName(_user);
        }

        [Benchmark]
        public async Task GetUserName_Interceptor()
        {
            await _userApp.GetUserNameWithEniymCacheInterceptorAsync(_user);
        }
    }
}
