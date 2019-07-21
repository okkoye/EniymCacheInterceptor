using System;
using System.Threading.Tasks;
using EniymCacheInterceptor.Demo.Web.Models;

namespace EniymCacheInterceptor.Demo.Web.Services
{
    public class TestService : ITestService
    {
        public async Task<Person> GetUser(int id)
        {
            var person = new Person(id, "tim" + id);
            return await Task.FromResult(person);
        }

        public async Task<string> GetUserName(Person p)
        {
            return await Task.FromResult<string>($"tim_{p.Id}_{Guid.NewGuid()}");
        }
    }
}