using System.Threading.Tasks;
using EniymCacheInterceptor;
using EniymCacheInterceptor.Demo.Web.Models;

namespace EniymCacheInterceptor.Demo.Web.Services
{
    public interface ITestService
    {
        Task<string> GetUserName(Person p);
        Task<string> GetUserNameWithMemoryCache(Person p);

        [EniymCacheGetOrCreate(Template = "user_address_{p:Address:Id}", CacheSeconds = 180)]
        Task<string> GetUserNameWithEniyCacheInterceptor(Person p);
    }
}