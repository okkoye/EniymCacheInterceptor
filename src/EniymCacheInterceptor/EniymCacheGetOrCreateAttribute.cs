using System;

namespace EniymCacheInterceptor
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class EniymCacheGetOrCreateAttribute : EniymCacheInterceptorAttribute
    {
        /// <summary>
        /// 缓存时间(单位秒)
        /// </summary>
        public int CacheSeconds { get; set; }
    }
}