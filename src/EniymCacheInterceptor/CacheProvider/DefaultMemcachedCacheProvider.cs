using System;
using System.Threading.Tasks;
using Enyim.Caching;
using Newtonsoft.Json.Linq;

namespace EniymCacheInterceptor.CacheProvider
{
    /// <summary>
    /// Memcached 缓存实现
    /// </summary>
    public class DefaultMemcachedCacheProvider : IEniymCacheProvider
    {
        private readonly IMemcachedClient _cache;

        public DefaultMemcachedCacheProvider(IMemcachedClient cache)
        {
            _cache = cache;
        }


        public async Task SetAsync(string cacheKey, object cacheValue, int cacheSeconds)
        {
            await _cache.AddAsync(cacheKey, cacheValue, cacheSeconds);
        }

        public async Task RemoveAsync(string cacheKey)
        {
            await _cache.RemoveAsync(cacheKey);
        }

        public async Task<object> GetAsync(string cacheKey, Type type)
        {
            if (!(await Task.FromResult(_cache.Get(cacheKey)) is JObject result))
            {
                return null;
            }

            return result.ToObject(type);
        }
    }
}