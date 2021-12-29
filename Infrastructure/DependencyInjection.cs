using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        private static readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration) 
        {
            services.AddDbContext<ReversiDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("ReversiDbConnection")));

            services.AddScoped<IReversiDbContext>(provider => provider.GetRequiredService<ReversiDbContext>());

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            services
                .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddHttpClient("SpelRestAPI", client =>
            {
                client.BaseAddress = new Uri("https://localhost:44339/api/");
            }).AddTransientHttpErrorPolicy(policy => 
                policy.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(300)));

            services.AddCors(options => 
            {
                options.AddPolicy(MyAllowSpecificOrigins, builder =>
                    {
                        builder.WithOrigins("https://localhost:44339/api/Spel");
                    });
            });

            services.AddSingleton<ISpelService, SpelService>();

            return services;
        }
    }
}
