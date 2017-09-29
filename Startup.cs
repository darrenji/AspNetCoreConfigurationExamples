using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AspNetCoreConfiguration.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace AspNetCoreConfiguration
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<UptimeService>();
            services.AddMvc().AddMvcOptions(options => {
                options.RespectBrowserAcceptHeader = true;
            });
        }

        //为开发阶段准备
        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            services.AddSingleton<UptimeService>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            #region 混在一起的写法
            //if(Configuration.GetSection("ShortCircuitMiddleware")?["EnableBrowserShortCircuit"] == "True")
            //{
            //    app.UseMiddleware<BrowserTypeMiddleware>();
            //    app.UseMiddleware<ShortCircuitMiddleware>();
            //}

            ////loggerFactory.AddConsole(LogLevel.Debug);
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug(LogLevel.Debug);

            ////当环境变量设置为Staging的时候，这里就不执行
            //if (env.IsDevelopment())
            //{
            //    //app.UseMiddleware<ErrorMiddleware>();
            //    //app.UseMiddleware<BrowserTypeMiddleware>();
            //    //app.UseMiddleware<ShortCircuitMiddleware>();
            //    //app.UseMiddleware<ContentMiddleware>();
            //    app.UseDeveloperExceptionPage();
            //    app.UseStatusCodePages();

            //    //开发阶段需要这个
            //    app.UseBrowserLink();
            //} else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //}

            //app.UseStaticFiles();
            ////app.UseMvcWithDefaultRoute()
            //app.UseMvc(routes => {
            //    routes.MapRoute(
            //        name:"default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //}); 
            #endregion

            app.UseExceptionHandler("/Home/Error");
            app.UseStaticFiles();
            app.UseMvc(routes => {
                routes.MapRoute(name:"default", template:"{controller=Home}/{action=Index}/{id?}");
            });

        }

        public void ConfigureDevelopment(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug(LogLevel.Debug);
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseBrowserLink();
            app.UseStaticFiles();
            app.UseMvc(routes => {
                routes.MapRoute(name:"default", template:"{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
