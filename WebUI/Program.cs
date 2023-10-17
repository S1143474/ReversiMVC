using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebUI;

namespace MyNamespace
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            IWebHost host = CreateWebHostBuilder(args).Build();
            IWebHost identitySeededHost = await host.SeedIdentityData();
            IWebHost spelerSeededHost = identitySeededHost.SeedData();
            spelerSeededHost.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            //"https://192.168.2.15:3000", 
            var builder = WebHost.CreateDefaultBuilder(args)
/*                .UseUrls("https://192.168.2.30:3000")
*/                .UseStartup<Startup>();

            builder.ConfigureLogging((Action<WebHostBuilderContext, ILoggingBuilder>)((hostingContext, logging) =>
            {
                logging.AddConfiguration((IConfiguration)hostingContext.Configuration.GetSection("Logging"));
                logging.AddConsole();
                logging.AddDebug();
                logging.AddEventSourceLogger();
            }));
            return builder;
        }
    }
}
