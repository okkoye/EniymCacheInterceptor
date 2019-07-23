using System.Threading.Tasks;

namespace EniymCacheInterceptor.UnitTests.Infrastructure
{
    public interface ITestAppService
    {
        Task<string> GetCurrentTime();

        [EniymCacheGetOrCreate(Template = "interceptor_current_time", CacheSeconds = 180)]
        Task<string> GetCurrentTimeWithInterceptorAsync();
    }
}