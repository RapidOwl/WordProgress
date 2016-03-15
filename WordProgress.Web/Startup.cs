using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Glimpse;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WordProgress.ReadModels;

namespace WordProgress.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            //if (env.IsDevelopment())
            //{
            //    // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
            //    builder.AddUserSecrets();

            //    // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
            //    builder.AddApplicationInsightsSettings(developerMode: true);
            //}

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGlimpse();

            services.AddMvc();

            services.AddTransient<IWriterReader, WriterReader>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseGlimpse();
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                //// For more details on creating database during deployment see http://go.microsoft.com/fwlink/?LinkID=615859
                //try
                //{
                //    using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                //        .CreateScope())
                //    {
                //        serviceScope.ServiceProvider.GetService<ApplicationDbContext>()
                //             .Database.Migrate();
                //    }
                //}
                //catch { }
            }

            app.UseIISPlatformHandler();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
