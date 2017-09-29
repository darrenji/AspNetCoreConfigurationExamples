using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace AspNetCoreConfiguration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel() //使用Kestrel服务器
                .UseContentRoot(Directory.GetCurrentDirectory()) //读出根目录下的配置文件和其它静态文件，比如image，css,js等
                .UseIISIntegration() //匹配IIS和IISExpress
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
