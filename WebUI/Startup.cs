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
using WebUI.Middleware;
using WebUI.Middleware.ExceptionMiddleWare;
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
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddApplication();
        services.AddInfrastructure(Configuration);
        services.AddAutoMapper(typeof(Startup));

        services.AddScoped<StillPlayingFilter>();
        services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
        });
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
        /*else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }*/

        app.UseMiddleware<ExceptionMiddleWare>();
        app.UseMiddleware<HideHiddenFilesMiddleware>();

        app.UseHttpsRedirection();

        var nonceGenerator = app.ApplicationServices.GetRequiredService<INonceGenerator>();

        app.Use(async (context, next) =>
        {
            context.Response.Headers.Add("X-Frame-Options", "DENY");
            context.Response.Headers.Add("Content-Security-Policy", 
                "default-src 'self'; " +
                $"script-src 'self' https://cdnjs.cloudflare.com https://www.google-analytics.com https://www.google.com https://www.gstatic.com https://icanhazdadjoke.com/ 'nonce-{nonceGenerator.GetNonce("Reversi")}' 'nonce-{nonceGenerator.GetNonce("Recaptcha")}' 'nonce-{nonceGenerator.GetNonce("jqueryvalidate")}' 'nonce-{nonceGenerator.GetNonce("jqueryvalidateunobstructive")}' 'nonce-{nonceGenerator.GetNonce("jqueryvalidatemin")}' 'nonce-{nonceGenerator.GetNonce("jqueryvalidateunobstructivemin")}'; " +
                "style-src 'self' fonts.googleapis.com; " +
                "font-src 'self' fonts.googleapis.com fonts.gstatic.com; " +
                "frame-src 'self' https://www.google.com/; " +
                "connect-src 'self' https://www.google.com; " +
                "frame-ancestors 'self'; " +
                "form-action 'self';"
            );
            await next();
        });

        app.UseStaticFiles();
        app.UseCookiePolicy();
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