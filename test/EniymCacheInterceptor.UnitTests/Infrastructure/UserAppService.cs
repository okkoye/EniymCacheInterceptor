using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace EniymCacheInterceptor.UnitTests.Infrastructure
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