using System.Threading.Tasks;
using EniymCacheInterceptor;
using EniymCacheInterceptor.Demo.Web.Models;

namespace EniymCacheInterceptor.Demo.Web.Services
{
    public interface ITestService
    {
        [EniymCacheGetOrCreate(Template = "user_{id}", CacheSeconds = 180)]
        Task<Person> GetUser(int id);

        [EniymCacheGetOrCreate(Template = "username_{id}", CacheSeconds = 180)]
        Task<string> GetUserName(int id);
    }
}