using System.Threading.Tasks;
using EniymCacheInterceptor;
using EniymCacheInterceptor.Demo.Web.Models;

namespace EniymCacheInterceptor.Demo.Web.Services
{
    public interface ITestService
    {
        [EniymCacheGetOrCreate(Template = "user_{id}", CacheSeconds = 180)]
        Task<Person> GetUser(int id);

        [EniymCacheGetOrCreate(Template = "user_{p:Id}_address_{p:Id:Address:Id}", CacheSeconds = 180)]
        Task<string> GetUserName(Person p);
    }
}