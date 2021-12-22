using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReversiMvcApp.Annotations;
using ReversiMvcApp.Data;
using ReversiMvcApp.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReversiMvcApp.Filters;

namespace ReversiMvcApp
{
    public class Startup
    {

        private readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ISpelService, SpelService>();

            services.AddMvc();
            services.AddScoped<StillPlayingFilter>();
           /* services.AddMvc().AddMvcOptions(options =>
            {
                options.Filters.Add(typeof(StillPlayingFilter));
            });*/

            services.AddSignalR();

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
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHub<ReversiHub>("/reversiHub");
                endpoints.MapRazorPages();
            });
        }
    }
}
