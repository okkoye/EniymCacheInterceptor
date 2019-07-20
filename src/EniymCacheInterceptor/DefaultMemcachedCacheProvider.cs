using System;
using System.Threading.Tasks;
using Enyim.Caching;

namespace EniymCacheInterceptor
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


        public async Task SetAsync<T>(string cacheKey, T cacheValue, int cacheSeconds)
        {
            await _cache.AddAsync(cacheKey, cacheValue, cacheSeconds);
        }

        public async Task RemoveAsync(string cacheKey)
        {
            await _cache.RemoveAsync(cacheKey);
        }

        public async Task<object> GetAsync(string cacheKey, Type type)
        {
            var result = await Task.FromResult(_cache.Get(cacheKey));
            if (result != null)
                return result;
            return null;
        }

        public object Get(string cacheKey, Type type)
        {
            return _cache.Get(cacheKey);
        }
    }
}