using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace EniymCacheInterceptor.CacheProvider
{
    /// <summary>
    /// Memory 实现
    /// </summary>
    public class DefaultMemoryCacheProvider : IEniymCacheProvider
    {
        private readonly IMemoryCache _cache;

        public DefaultMemoryCacheProvider(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task SetAsync(string cacheKey, object cacheValue, int cacheSeconds)
        {
            var expired = DateTime.Now.AddSeconds(cacheSeconds);
            await Task.FromResult(_cache.Set(cacheKey, cacheValue, expired));
        }

        public async Task RemoveAsync(string cacheKey)
        {
            _cache.Remove(cacheKey);
            await Task.FromResult(0);
        }

        public async Task<object> GetAsync(string cacheKey, Type type)
        {
            return await Task.FromResult(_cache.Get(cacheKey));
        }
    }
}