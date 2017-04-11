using System.Collections.Generic;
using AW.Application.Services.Identity;
using AW.Application.Services.Identity.Logger;
using AW.Common.GuardToolkit;
using AW.Common.PersianToolkit;
using AW.Common.WebToolkit;
using AW.DataLayer.Context;
using AW.DataLayer.Settings;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AW.Presentation
{
    public class Startup
    {
        public IConfigurationRoot Configuration { set; get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                                .SetBasePath(env.ContentRootPath)
                                .AddInMemoryCollection(new[]
                                {
                                    new KeyValuePair<string,string>("the-key", "the-value")
                                })
                                .AddJsonFile("appsettings.json", reloadOnChange: true, optional: false)
                                .AddJsonFile($"appsettings.{env}.json", true)
                                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton(provider => Configuration);
            services.Configure<SiteSettings>(options => Configuration.Bind(options));

            services.AddDbContext<ApplicationDbContext>(ServiceLifetime.Scoped);

            // Adds all of the ASP.NET Core Identity related services and configurations at once.
            services.AddCustomIdentityServices();

            services.AddMvc(options =>
            {
                options.UseCustomStringModelBinder();
            }).AddJsonOptions(jsonOptions =>
            {
                jsonOptions.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });

            services.AddRazorViewRenderer();
            services.AddDNTCaptcha();
            services.AddMvcActionsDiscoveryService();
            services.AddProtectionProviderService();
            services.AddCloudscribePagination();
        }

        public void Configure(
            ILoggerFactory loggerFactory,
            IApplicationBuilder app,
            IHostingEnvironment env)
        {
            loggerFactory.AddDbLogger(app.ApplicationServices, LogLevel.Warning);

            app.UseExceptionHandler("/error/index/500");
            app.UseStatusCodePagesWithReExecute("/error/index/{0}");

            // Serve wwwroot as root
            app.UseFileServer();


            // Adds all of the ASP.NET Core Identity related initializations at once.
            app.UseCustomIdentityServices();

            app.UseMvc(routes =>
            {
                
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
