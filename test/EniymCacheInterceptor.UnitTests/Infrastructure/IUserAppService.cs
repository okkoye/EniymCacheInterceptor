using System.Threading.Tasks;

namespace EniymCacheInterceptor.UnitTests.Infrastructure
{
    public interface IUserAppService
    {
        Task<string> GetUserName(User user);

        [EniymCacheGetOrCreate(Template = "user_2_{user:Id}", CacheSeconds = 180)]
        Task<string> GetUserNameWithEniymCacheInterceptorAsync(User user);
    }
}
