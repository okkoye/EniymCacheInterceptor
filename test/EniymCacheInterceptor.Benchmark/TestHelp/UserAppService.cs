using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EniymCacheInterceptor.Benchmark
{
    public class UserAppService : IUserAppService
    {
        private readonly IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        public async Task<string> GetUserName(User user)
        {
            var cacheKey = $"user_{user.Id}";
            var cacheValue = await Task.FromResult(_cache.Get<string>(cacheKey));
            if (cacheValue != null)
            {
                return cacheValue;
            }
            var expired = DateTime.Now.AddSeconds(180);
            _cache.Set<string>(cacheKey, user.Name, expired);
            return user.Name;
        }

        public async Task<string> GetUserNameWithEniymCacheInterceptorAsync(User user)
        {
            return await Task.FromResult(user.Name);
        }
    }
}
