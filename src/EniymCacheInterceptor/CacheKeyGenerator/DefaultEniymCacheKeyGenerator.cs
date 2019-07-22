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
            var placeholderList = GetPlaceholderList(cacheKeyTemplate);
            if (!placeholderList.Any())
                return cacheKeyTemplate;

            var parameterDic = GetParameterDictoryFromContext(context);
            foreach (var placeholder in placeholderList)
            {
                if (placeholder.Contains(":"))
                {
                    var temp = placeholder.Split(':');
                    var rootParameterName = temp[0];
                    object objetValue;

                    if (!parameterDic.TryGetValue(rootParameterName, out objetValue))
                        throw new Exception($"can not found root parameter '{rootParameterName}'");

                    for (int i = 1; i < temp.Length; i++)
                    {
                        var ob = ConvertHelper.ToDictionary(objetValue);
                        if (!ob.TryGetValue(temp[i], out objetValue))
                            throw new Exception($"can not fount parameter {temp[i]}");
                    }

                    cacheKeyTemplate = cacheKeyTemplate.Replace("{" + placeholder + "}", objetValue.ToString());
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

            return cacheKeyTemplate;
        }

        /// <summary>
        /// 获取缓存模板中的占位符
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        private List<string> GetPlaceholderList(string template)
        {
            var placeholderList = new List<string>();
            var matchs = Regex.Matches(template, @"\{(.*)\}", RegexOptions.Compiled);
            foreach (Match match in matchs)
            {
                if (match.Success)
                {
                    placeholderList.Add(match.Value.TrimStart('{').TrimEnd('}'));
                }
            }

            return placeholderList;
        }

        /// <summary>
        /// 方法参数放进字典
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private Dictionary<string, object> GetParameterDictoryFromContext(AspectContext context)
        {
            var parameterDic = new Dictionary<string, object>();
            var parameterInfos = context.ProxyMethod.GetParameters();
            if (!parameterInfos.Any())
                return parameterDic;

            for (var i = 0; i < parameterInfos.Length; i++)
            {
                parameterDic.Add(parameterInfos[i].Name, context.Parameters[i]);
            }

            return parameterDic;
        }
    }
}