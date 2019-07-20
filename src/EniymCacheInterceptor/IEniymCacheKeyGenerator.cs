using AspectCore.DynamicProxy;

namespace EniymCacheInterceptor
{
    /// <summary>
    /// 自定义键值的生成
    /// </summary>
    public interface IEniymCacheKeyGenerator
    {
        /// <summary>
        /// 获取缓存键值
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cacheKeyTemplate">缓存键模板</param>
        /// <returns></returns>
        string GetCacheKey(AspectContext context, string cacheKeyTemplate);
    }
}