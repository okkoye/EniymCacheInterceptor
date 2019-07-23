using System;

namespace EniymCacheInterceptor
{
    /// <summary>
    /// 默认缓存抽象
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class EniymCacheInterceptorAttribute : Attribute
    {
        /// <summary>
        /// 缓存键模板
        /// </summary>
        public string Template { get; set; } = string.Empty;
    }
}