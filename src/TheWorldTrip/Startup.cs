using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using TheWorld.Services;
using TheWorldTrip.Models;
using TheWorldTrip.Services;
using TheWorldTrip.ViewModels;

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
            

            if(_env.IsEnvironment("Development"))
            {
                services.AddSingleton<IMailService, DebugMailService>();
                
            }
            else
            {
                // Implement a real Mail Service
            }

            services.AddMvc().AddJsonOptions(config =>
            {
                config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });



            services.AddSingleton(_config);

            services.AddDbContext<TheWorldTripContext>();

            services.AddScoped<ITripRepository, TripRepository>();

            services.AddTransient<TheWorldTripContextSeedData>();
            services.AddTransient<GeoCoordsService>();
            
            services.AddLogging();

           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
                IApplicationBuilder app, 
                IHostingEnvironment env, 
                TheWorldTripContextSeedData seeder,
                ILoggerFactory factory)
        {

            AutoMapper.Mapper.Initialize(config => {
                config.CreateMap<TripViewModel, Trip>().ReverseMap();
                config.CreateMap<StopViewModel, Stop>().ReverseMap();
            });




            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                factory.AddDebug(LogLevel.Information);
            }
            else
            {
                factory.AddDebug(LogLevel.Error);
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

            seeder.EnsureSeedData().Wait();
        }
    }
}
