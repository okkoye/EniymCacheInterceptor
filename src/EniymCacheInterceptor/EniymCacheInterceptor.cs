using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using AspectCore.Extensions.Reflection;
using AspectCore.Injector;
using Microsoft.Extensions.Logging;

namespace EniymCacheInterceptor
{
    /// <summary>
    /// 自定义缓存拦截器
    /// </summary>
    public class EniymCacheInterceptor : AbstractInterceptor
    {
        [FromContainer] private IEniymCacheKeyGenerator KeyGenerator { get; set; }
        [FromContainer] private IEniymCacheProvider CacheProvider { get; set; }

        [FromContainer] public ILogger<EniymCacheInterceptor> Logger { get; set; }

        private static readonly ConcurrentDictionary<Type, MethodInfo>
            TaskResultMethod = new ConcurrentDictionary<Type, MethodInfo>();

        private static readonly ConcurrentDictionary<MethodInfo, Attribute>
            MethodAttributes = new ConcurrentDictionary<MethodInfo, Attribute>();

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            await ProcessGetOrCreateAsync(context, next);
        }

        private Attribute GetMethodAttributes(MethodInfo mi)
        {
            return MethodAttributes.GetOrAdd(mi,
                mi.GetReflector().GetCustomAttribute(typeof(EniymCacheGetOrCreateAttribute)));
        }

        private async Task ProcessGetOrCreateAsync(AspectContext context, AspectDelegate next)
        {
            if (GetMethodAttributes(context.ServiceMethod) is EniymCacheGetOrCreateAttribute attribute)
            {
                if (string.IsNullOrEmpty(attribute.Template))
                    throw new ArgumentNullException($"please set the cache key '{nameof(attribute.Template)}'");

                var returnType = context.IsAsync()
                    ? context.ServiceMethod.ReturnType.GetGenericArguments().First()
                    : context.ServiceMethod.ReturnType;

                var cacheKey = KeyGenerator.GetCacheKey(context, attribute.Template);

                object cacheValue = null;
                try
                {
                    cacheValue = await CacheProvider.GetAsync(cacheKey, returnType);
                }
                catch
                {
                    Logger.LogError($"An error occurred while reading cache '{cacheKey}'.");
                    cacheValue = null;
                }

                if (cacheValue != null)
                {
                    context.ReturnValue = context.IsAsync()
                        ? TaskResultMethod
                            .GetOrAdd(returnType,
                                t => typeof(Task).GetMethods()
                                    .First(p => p.Name == "FromResult" && p.ContainsGenericParameters)
                                    .MakeGenericMethod(returnType)).Invoke(null, new object[] {cacheValue})
                        : cacheValue;
                }
                else
                {
                    await next(context);

                    var returnValue = context.IsAsync()
                        ? await context.UnwrapAsyncReturnValue()
                        : context.ReturnValue;

                    if (returnValue != null)
                    {
                        await CacheProvider.SetAsync(cacheKey, returnValue, attribute.CacheSeconds);
                    }
                }
            }
            else
            {
                await next(context);
            }
        }
    }
}