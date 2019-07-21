using System;
using System.Threading.Tasks;

namespace EniymCacheInterceptor
{
    /// <summary>
    /// EniymCacheProvider
    /// </summary>
    public interface IEniymCacheProvider
    {
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="cacheValue"></param>
        /// <param name="cacheSeconds"></param>
        /// <returns></returns>
        Task SetAsync(string cacheKey, object cacheValue, int cacheSeconds);

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        Task RemoveAsync(string cacheKey);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<object> GetAsync(string cacheKey, Type type);
    }
}