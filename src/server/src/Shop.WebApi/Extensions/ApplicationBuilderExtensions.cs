using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shop.Infrastructure.Modules;
using System;
using System.Linq;

namespace Shop.WebApi.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseCustomizedConfigure(this IApplicationBuilder app, IConfiguration config, IWebHostEnvironment env)
        {
            var originals = config.GetValue<string[]>("Cors:Originals") ?? new string[] { "*" };
            // 跨域
            app.UseCors(builder =>
            {
                builder.AllowAnyMethod().AllowAnyHeader()
                .SetIsOriginAllowed(orignal => originals.Contains("*") || originals.Contains(orignal))
                .AllowCredentials();
            });

            // 静态资源
            app.UseStaticFiles();

            // 模块
            var moduleInitializers = app.ApplicationServices.GetServices<IModuleInitializer>();
            foreach (var moduleInitializer in moduleInitializers)
            {
                moduleInitializer.Configure(app, env);
            }
        }
    }
}
