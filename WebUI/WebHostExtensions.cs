using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyNamespace;
using System;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;

namespace WebUI
{
    public static class WebHostExtensions
    {
        public static IWebHost SeedData(this IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<ReversiDbContext>();

                    var seeder = new ReversiDbInitializer(context);

                    seeder.Seed();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();

                    logger.LogCritical($"An error occurred initializing the database: {ex}");
                }
            }

            return host;
        }
    }
}
