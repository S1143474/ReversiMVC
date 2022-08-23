using Application;
using Application.Common.Interfaces;
using Application.Hubs;
using Infrastructure;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using WebUI;
using WebUI.Filters;
using WebUI.Services;

internal class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        services.AddApplication();
        services.AddInfrastructure(Configuration);
        services.AddAutoMapper(typeof(Startup));

        services.AddScoped<StillPlayingFilter>();
        /* services.AddMvc().AddMvcOptions(options =>
         {
             options.Filters.Add(typeof(StillPlayingFilter));
         });*/

        services.AddMvc();

        services.AddSession();

        services.AddControllersWithViews();
        services.AddRazorPages();

        services.Configure<GoogleCaptchaConfig>(Configuration.GetSection("Recaptcha"));
        /*services.AddRecaptcha(new RecaptchaOptions
        {
            SiteKey = Configuration["Recaptcha:SiteKey"],
            SecretKey = Configuration["Recaptcha:SecretKey"]
        });*/
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            /*app.UseDatabaseErrorPage();*/
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

            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Spel}/{action=AvailableGames}/{id?}");

            endpoints.MapHub<ReversiHub>("/reversiHub");

            endpoints.MapRazorPages();
        });

        loggerFactory.AddFile("Logs/reversi-mvc-{Date}.txt");

    }
}