using System;
using System.Linq;
using System.Reflection;
using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using EniymCacheInterceptor.CacheKeyGenerator;
using EniymCacheInterceptor.CacheProvider;
using EniymCacheInterceptor.Demo.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EniymCacheInterceptor.Demo.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ITestService, TestService>();
            services.AddEnyimMemcached();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.TryAddSingleton<IEniymCacheProvider, DefaultMemoryCacheProvider>();
            services.TryAddSingleton<IEniymCacheKeyGenerator, DefaultEniymCacheKeyGenerator>();

            bool All(MethodInfo x) =>
                x.CustomAttributes.Any(data => typeof(EniymCacheInterceptorAttribute)
                    .IsAssignableFrom(data.AttributeType));

            services.ConfigureDynamicProxy(config => { config.Interceptors.AddTyped<EniymCacheInterceptor>(All); });

            return services.BuildAspectInjectorProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseEnyimMemcached();
        }
    }
}