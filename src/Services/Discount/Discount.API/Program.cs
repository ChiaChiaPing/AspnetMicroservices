using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discount.API.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Discount.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            // has already register service to IoC container.
            var host = CreateHostBuilder(args);

            // define the one-shot implementation like initialization like conn check, table creatioon and data preparation
            // user builder pattern with ExxtensionMethods

            // Build will go through StartUp Constructor and Configure Services     
            host.Build()
                .MigrateDatabase<Program>()
                .Run(); //RUn will go through Configure
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
