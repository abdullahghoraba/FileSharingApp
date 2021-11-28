using FileSharingApp.Areas.Admin.Services;
using FileSharingApp.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileSharingApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host=CreateHostBuilder(args).Build();
            // Run migration >> update-database like on local but this on server
            // here i need to arrive to db Context so i get it from service container 

            using(var scope = host.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                // this two lines in production
                //var DbContext = provider.GetRequiredService<ApplicationContext>();
                //DbContext.Database.Migrate();
                var userService = provider.GetRequiredService<IUserService>();
                await userService.InitializeAsync();
            }
            // we can seed data here 
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
