using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discount.Grpc.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;

namespace Discount.Grpc
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

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                    {
                     
                    }).UseStartup<Startup>();
                });

    }
}
