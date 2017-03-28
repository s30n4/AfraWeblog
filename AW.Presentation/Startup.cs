using AW.DataLayer.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Diagnostics;
using StructureMap;

namespace AW.Presentation
{
    public class Startup
    {
        private IConfigurationRoot Configuration { set; get; }
        private IHostingEnvironment Environment { set; get; }
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", reloadOnChange: true, optional: false)
                .AddJsonFile($"appsettings.{env}.json", true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            Environment = env;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(provider => Configuration);
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:SaminDbConnection"]);
            });

            services.AddMvc()
                .AddJsonOptions(
                    options =>
                            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());


            var container = new Container();


            services.AddSingleton(provider => Configuration);
            ServiceIoc.Ioc(services);
            string configName = $"{Environment.ContentRootPath}\\config.developments.json";

          

            var configuration = new ConfigurationBuilder()
                //.AddJsonFile(Environment.ContentRootPath + "/Config.json", false, true)
                .AddJsonFile(configName, false, true)
                .Build();

            //if (configuration.GetValue<bool>("CorsConfig:UseMicrosoftCors"))
            //{
            //    services.AddCors(options =>
            //    {
            //        options.AddPolicy("AllowSpecificOrigin",
            //            builder =>
            //                builder.WithOrigins(configuration.GetValue<string>("CorsConfig:AllowOrigins:0"),
            //                        configuration.GetValue<string>("CorsConfig:AllowOrigins:1"),
            //                        configuration.GetValue<string>("CorsConfig:AllowOrigins:2"),
            //                        configuration.GetValue<string>("CorsConfig:AllowOrigins:3"), configuration.GetValue<string>("CorsConfig:AllowOrigins:4"))
            //                    .AllowAnyHeader()
            //                    .AllowAnyMethod());
            //    });

            //    services.Configure<MvcOptions>(options =>
            //    {
            //        options.Filters.Add(new CorsAuthorizationFilterFactory("AllowSpecificOrigin"));
            //    });
            //}

            ////if (configuration.GetValue<bool>("CorsConfig:UseCors"))
            //{
            //    CorsService.Add(configuration.GetValue<string>("CorsConfig:CorsProfile"), x =>
            //    {
            //        x.AllowAnyHeader().AllowAnyMethod().WithOrigins(
            //            configuration.GetValue<string>("CorsConfig:AllowOrigins:0"),
            //            configuration.GetValue<string>("CorsConfig:AllowOrigins:1"),
            //            configuration.GetValue<string>("CorsConfig:AllowOrigins:3"),
            //            configuration.GetValue<string>("CorsConfig:AllowOrigins:2"));
            //    });
            //}

            container.Populate(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IServiceScopeFactory scopeFactory, IServiceProvider provider)
        {
          //  scopeFactory.SeedData();
            app.UseStaticFiles(); // For the wwwroot folder
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), @"Uploads")),
                RequestPath = new PathString("/Content")
            });




            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"Uploads")),
                RequestPath = new PathString("/Content")
            });

            if (env.IsDevelopment())
            {
                app.UseDatabaseErrorPage();
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/Home";
                    await next();
                }
            });

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            // Register a simple error handler to catch token expiries and change them to a 401,
            // and return all other errors as a 500. This should almost certainly be improved for
            // a real application.
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Use(async (context, next) =>
                {
                    var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;
                    if (error?.Error != null && context.Response.StatusCode != 200)
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";
                        // TODO: Shouldn't pass the exception message straight out, change this.
                        context.Response.Headers["Access-Control-Allow-Origin"] = "*";
                        await context.Response.WriteAsync(
                            JsonConvert.SerializeObject
                                (new { success = false, error = error.Error.Message }));

                    }
                    // We're not trying to handle anything else so just let the default
                    // handler handle.
                    else await next();
                });
            });

            string configName = $"{Environment.ContentRootPath}\\config.developments.json";

         

            var configuration = new ConfigurationBuilder()
                //.AddJsonFile(Environment.ContentRootPath + "/Config.json", false, true)
                .AddJsonFile(configName, false, true)
                .Build();


      
            // app.UseResponseBuffering();

            //if (configuration.GetValue<bool>("CorsConfig:UseCors"))
            //{
            //    app.UseCorsOrigin(configuration.GetValue<string>("CorsConfig:CorsProfile"));
            //}

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                 "default",
                 "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute("spa-fallback", new { controller = "Home", action = "Index" });
            });

        }
    }
}
