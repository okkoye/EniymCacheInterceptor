using System;
using System.Threading.Tasks;
using EniymCacheInterceptor.Demo.Web.Models;
using Microsoft.Extensions.Caching.Memory;

namespace EniymCacheInterceptor.Demo.Web.Services
{
    public class TestService : ITestService
    {
        private readonly IMemoryCache _cache;

        public TestService(IMemoryCache cache)
        {
            _cache = cache;
        }
        public async Task<string> GetUserName(Person p)
        {
            return await Task.FromResult(p.Name);
        }

        public async Task<string> GetUserNameWithMemoryCache(Person p)
        {
            var cacheKey = $"user_{p.Id}";
            var cacheValue = await Task.FromResult(_cache.Get<string>(cacheKey));
            if (cacheValue != null)
            {
                return cacheValue;
            }
            var expired = DateTime.Now.AddSeconds(180);
            _cache.Set(cacheKey, p.Name, expired);
            return p.Name;
        }

        public async Task<string> GetUserNameWithEniyCacheInterceptor(Person p)
        {
            return await Task.FromResult<string>($"tim_{p.Name}_{Guid.NewGuid()}");
        }
    }
}