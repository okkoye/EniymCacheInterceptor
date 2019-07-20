using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using AspectCore.Injector;

namespace EniymCacheInterceptor
{
    /// <summary>
    /// 自定义缓存拦截器
    /// </summary>
    public class EniymCacheInterceptor : AbstractInterceptor
    {
        [FromContainer] private IEniymCacheKeyGenerator KeyGenerator { get; set; }

        [FromContainer] private IEniymCacheProvider CacheProvider { get; set; }

        private static readonly ConcurrentDictionary<Type, MethodInfo>
            TaskResultMethod = new ConcurrentDictionary<Type, MethodInfo>();

        private static readonly ConcurrentDictionary<MethodInfo, object[]>
            MethodAttributes = new ConcurrentDictionary<MethodInfo, object[]>();

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            await ProcessGetOrCreateAsync(context, next);
        }

        private object[] GetMethodAttributes(MethodInfo mi)
        {
            return MethodAttributes.GetOrAdd(mi, mi.GetCustomAttributes(true));
        }

        private async Task ProcessGetOrCreateAsync(AspectContext context, AspectDelegate next)
        {
            if (GetMethodAttributes(context.ServiceMethod)
                    .FirstOrDefault(x => x.GetType() == typeof(EniymCacheGetOrCreateAttribute)) is
                EniymCacheGetOrCreateAttribute
                attribute)
            {
                if (string.IsNullOrEmpty(attribute.Template))
                {
                    throw new ArgumentNullException($"{nameof(attribute.Template)}");
                }

                var cacheKey =
                    KeyGenerator.GetCacheKey(context, attribute.Template);
                var returnType = context.IsAsync()
                    ? context.ServiceMethod.ReturnType.GetGenericArguments().First()
                    : context.ServiceMethod.ReturnType;

                var cacheValue = CacheProvider.Get(cacheKey, returnType);
                if (cacheValue != null)
                {
                    if (context.IsAsync())
                    {
                        context.ReturnValue =
                            TaskResultMethod
                                .GetOrAdd(returnType,
                                    t => typeof(Task).GetMethods()
                                        .First(p => p.Name == "FromResult" && p.ContainsGenericParameters)
                                        .MakeGenericMethod(returnType)).Invoke(null, new object[] {cacheValue});
                    }
                    else
                    {
                        context.ReturnValue = cacheValue;
                    }
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