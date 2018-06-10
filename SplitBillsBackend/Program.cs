using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace SplitBillsBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //BuildWebHost(args).Run();

            //    var builder = new ConfigurationBuilder()
            //        .SetBasePath(env.ContentRootPath)
            //        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            //        .AddEnvironmentVariables();
            //    Configuration = builder.Build();

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var host = new WebHostBuilder()
                 .UseKestrel()
                 .UseContentRoot(Directory.GetCurrentDirectory())
                 .UseUrls(environment == "Development" ? "http://localhost:50000" : "http://192.168.0.81:59987")
                 .UseConfiguration(new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build()
                  )
                 .UseIISIntegration()
                 .UseStartup<Startup>()
                 .Build();

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
