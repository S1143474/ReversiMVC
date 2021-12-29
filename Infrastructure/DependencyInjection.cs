using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            /*services.AddSingleton<ISpelService, SpelService>();

            services.AddMvc();
            services.AddScoped<StillPlayingFilter>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Add reversidbContext
            services.AddDbContext<ReversiDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("ReversiDbConnection")));

            services.AddCors(options => {
                // Options for Cors
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:44339/api/Spel");
                    });
            });

            //services.AddDistributedMemoryCache();
            services.AddSession();

            services.AddSignalR();

            // Add Service for api Connection
            services.AddSingleton<ISpelService, SpelService>();

            services.AddControllersWithViews();
            services.AddRazorPages();*/

            return services;
        }
    }
}
