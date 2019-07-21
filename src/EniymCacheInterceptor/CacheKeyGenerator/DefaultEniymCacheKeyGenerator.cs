using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AspectCore.DynamicProxy;

namespace EniymCacheInterceptor.CacheKeyGenerator
{
    /// <summary>
    /// 默认缓存键生成
    /// </summary>
    public class DefaultEniymCacheKeyGenerator : IEniymCacheKeyGenerator
    {
        public string GetCacheKey(AspectContext context, string cacheKeyTemplate)
        {
            var placeholderList = new List<string>();
            var matchs = Regex.Matches(cacheKeyTemplate, @"\{\w*\:?\w*\}", RegexOptions.None);
            foreach (Match match in matchs)
            {
                if (match.Success)
                {
                    placeholderList.Add(match.Value.TrimStart('{').TrimEnd('}'));
                }
            }

            var parameterInfos = context.ProxyMethod.GetParameters();
            var parameterDic = new Dictionary<string, object>();
            if (placeholderList.Any())
            {
                for (var i = 0; i < parameterInfos.Length; i++)
                {
                    parameterDic.Add(parameterInfos[i].Name, context.Parameters[i]);
                }

                foreach (var placeholder in placeholderList)
                {
                    if (placeholder.Contains("."))
                    {
                        var temp = placeholder.Split('.');
                        var parameterName = temp[0];
                        var fieldName = temp[1];

                        if (!parameterDic.TryGetValue(parameterName, out var objetValue))
                        {
                            throw new Exception($"can not found parameter '{parameterName}'");
                        }

                        var ob = FastConvertHelper.ToDictionary(objetValue);
                        if (!ob.TryGetValue(fieldName, out var tokenValue))
                        {
                            throw new Exception($"can not fount field {fieldName}");
                        }

                        cacheKeyTemplate = cacheKeyTemplate.Replace("{" + placeholder + "}", tokenValue.ToString());
                    }
                    else
                    {
                        if (!parameterDic.TryGetValue(placeholder, out var value))
                        {
                            throw new Exception($"can not fount field {placeholder}");
                        }

                        cacheKeyTemplate = cacheKeyTemplate.Replace("{" + placeholder + "}", value.ToString());
                    }
                }
            }

            return cacheKeyTemplate;
        }
    }
}