using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using TowaStandard;
using Odyssey2Backend.BackgroundTasks;
using Microsoft.Extensions.DependencyInjection;

namespace Odyssey2Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            String strTest = Test.ToLog("abc");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices(services => 
                {
                    services.AddHostedService<BackgroundTasks.AlertservAlertService>();
                });
    }
}
