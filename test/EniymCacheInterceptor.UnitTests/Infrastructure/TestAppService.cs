using System;
using System.Threading.Tasks;

namespace EniymCacheInterceptor.UnitTests.Infrastructure
{
    public class TestAppService : ITestAppService
    {
        public async Task<string> GetCurrentTime()
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetCurrentTimeWithInterceptorAsync()
        {
            return DateTime.Now.ToString();
        }
    }
}