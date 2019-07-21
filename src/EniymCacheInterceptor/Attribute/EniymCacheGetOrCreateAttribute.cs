using System;

namespace EniymCacheInterceptor
{
    /// <summary>
    /// 缓存特性：先去缓存中取数据，没有再去执行查询方法
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class EniymCacheGetOrCreateAttribute : EniymCacheInterceptorAttribute
    {
        /// <summary>
        /// 缓存时间(单位秒)，默认30秒
        /// </summary>
        public int CacheSeconds { get; set; } = 30;
    }
}