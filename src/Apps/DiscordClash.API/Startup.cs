using DiscordClash.API.Extensions;
using EasyNetQ;
using Figgle;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;

namespace DiscordClash.API
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton(RabbitHutch.CreateBus(Configuration["rabbitMq:connectionString"]));
            services.AddConfiguredSwagger()
                .AddServices()
                .AddInfrastructure();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddHealthChecks()
                .AddRabbitMQ(rabbitConnectionString: Configuration["rabbitMq:connectionString"]);

            DisplayBanner();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Generated swagger json and swagger ui middleware
            app.UseCustomSwagger();

            // Global cors policy
            app.UseCors(x => x
                .SetIsOriginAllowed(_ => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithExposedHeaders("Location"));

            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCustomEndpoints();
        }

        private static void DisplayBanner()
        {
            var name = Assembly.GetCallingAssembly().GetName().Name;
            Console.WriteLine(FiggleFonts.Doom.Render(name!));
        }
    }
}
