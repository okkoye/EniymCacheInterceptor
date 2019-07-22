using System;
using System.Diagnostics;
using System.Threading.Tasks;
using EniymCacheInterceptor.Demo.Web.Models;
using EniymCacheInterceptor.Demo.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EniymCacheInterceptor.Demo.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITestService _testService;
        private readonly Person person = new Person()
        {
            Id = 100,
            Name = "HelloTim",
            DateAdded = DateTime.Now,
            Address = new Address()
            {
                Id = 101,
                Name = "浙江"
            }
        };
        public HomeController(ITestService testService)
        {
            _testService = testService;
        }

        public async Task<IActionResult> Test1()
        {
            return Ok(await _testService.GetUserName(person));
        }

        public async Task<IActionResult> Test2()
        {
            return Ok(await _testService.GetUserNameWithMemoryCache(person));
        }

        public async Task<IActionResult> Test3()
        {
            return Ok(await _testService.GetUserNameWithEniyCacheInterceptor(person));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}