using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ProductApp
{
    public class Program
    {
        // main entry point for aour app
        public static void Main(string[] args)
        {
            // custom method to create a web host.
            // Question: Why create a Web HOST in our application?
            // The reason is: This application can be hosted in non-Windows environment too
            // as .NEt Core is cross-platform
            // IIS will not be there on non-Windows environment
            // so we need a light weight web server to be created-> it is called Kestrel
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            // Creates Kestrel server instance and 
            Host.CreateDefaultBuilder(args)
            // configures it
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // using configuration and other things mentioned in Startup class
                    webBuilder.UseStartup<Startup>();
                });
        // lets go to Startup class and explore it
    }
}
