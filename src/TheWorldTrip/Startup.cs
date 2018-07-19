using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TheWorldTrip.Models;
using TheWorldTrip.Services;

namespace TheWorldTrip
{
    public class Startup
    {
        IHostingEnvironment _env;
        IConfigurationRoot _config;
        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(_env.ContentRootPath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables(); // Get Data from Environment Variable

            _config = builder.Build();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_config);

            if(_env.IsEnvironment("Development"))
            {
                services.AddSingleton<IMailService, DebugMailService>();
            }
            else
            {
                // implament production services
            }

            services.AddMvc();

            services.AddDbContext<TheWorldTripContext>();

           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //if(env.IsEnvironment("Development"))
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            app.UseStaticFiles();
            app.UseMvc(config =>
            {
                config.MapRoute(
                  name: "Default",
                  template: "{controller}/{action}/{id?}",
                  defaults: new { controller = "App", action = "Index" }
                  );
            });
        }
    }
}
