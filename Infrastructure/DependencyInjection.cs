using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interfaces;
using Infrastructure.Common;
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
                .AddDefaultIdentity<IdentityUser>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 12;

                    options.SignIn.RequireConfirmedAccount = true;
                    options.User.RequireUniqueEmail = true;

                    /* SET Rules for lockouts when entering a password to many times. (also preventing ddos attacks) */
                    options.Lockout.AllowedForNewUsers = true;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(double.Parse(configuration.GetSection("AppSettings:LockoutTimeInMinutes").Value));
                    options.Lockout.MaxFailedAccessAttempts = int.Parse(configuration.GetSection("AppSettings:LockoutMaxFailedAttempts").Value);
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            /*                .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>(TokenOptions.DefaultProvider)
             *                
            */
            /*                    .AddRoles<IdentityRole>();
            */
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });
            services.Configure<PasswordHasherOptions>(options => options.IterationCount = int.Parse(configuration.GetSection("AppSettings:HashIterations").Value));

            services.AddHttpClient("SpelRestAPI", client =>
            {
                client.BaseAddress = new Uri(configuration.GetValue<string>("ReversiRestAPI"));
                client.DefaultRequestHeaders.Add("x-api-key", configuration.GetValue<string>("ApiKey"));

            }).AddTransientHttpErrorPolicy(policy =>
                policy.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(300))
            );

            services.AddHttpClient("GoogleCaptcha", client =>
            {
                client.BaseAddress = new Uri("https://www.google.com/recaptcha/api/siteverify");
            });

            services.AddCors(options =>
                {
                options.AddPolicy(MyAllowSpecificOrigins, builder =>
                {
                    builder.WithOrigins("https://localhost:44339/api/Spel");
                    builder.WithOrigins("https://icanhazdadjoke.com/").WithMethods("*").WithHeaders("*");
                });
                
                options.AddDefaultPolicy(builder => builder.WithOrigins("https://localhost:44339", "https://icanhazdadjoke.com/").AllowCredentials());
            });

            /*  services.AddAuthorization(options =>
              {
                  options.AddPolicy("Moderator", policy => policy.RequireClaim("Moderator"));
                  options.AddPolicy("Admin", policy => policy.RequireClaim("Admin"));
              });*/

/*            services.AddAuthorization(options => options.AddPolicy(options.DefaultPolicy));
*/
            services.AddSingleton<ISpelService, SpelService>();
            services.AddScoped<IGoogleCaptchaService, GoogleCaptchaService>();

            services.AddScoped<DbContext, ReversiDbContext>();

            services.AddSignalR();


            services.Configure<ApplicationSettings>(configuration.GetSection("AppSettings"));
            services.AddScoped<IEmailService, EmailService>();

            

            return services;
        }
    }
}
